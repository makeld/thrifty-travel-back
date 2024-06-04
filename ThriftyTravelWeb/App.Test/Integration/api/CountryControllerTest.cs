using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using App.DTO.v1_0.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using Xunit.Abstractions;

namespace App.Test.Integration.api;

[Collection("NonParallel")]
public class CountryControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly JsonSerializerOptions _camelCaseJsonSerializerOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public CountryControllerTest(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
    {
        _factory = factory;
        _testOutputHelper = testOutputHelper;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    private async Task<string> LoginAndCreateUser()
    {
        const string url = "/api/v1/identity/account/register?expiresInSeconds=123";
        var email = $"{Guid.NewGuid()}@test.ee";
        const string password = "Foo123Bar!";

        var register = new RegisterInfo()
        {
            Email = email,
            Password = password,
            Firstname = "Test",
            Lastname = "User"
        };
        var data = JsonContent.Create(register);

        var response = await _client.PostAsync(url, data);
        var responseContent = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"JWT Token: {responseContent}");
        Assert.True(response.IsSuccessStatusCode);

        return responseContent;
    }

    [Fact]
    public async Task GetCountriesRequiresLogin()
    {
        var response = await _client.GetAsync("/api/v1/Countries");
        _testOutputHelper.WriteLine($"Status Code: {response.StatusCode}");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact(DisplayName = "GET - get all countries")]
    public async Task GetAllCountries()
    {
        var responseContent = await LoginAndCreateUser();
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;

        var url = "/api/v1/Countries";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);

        var response = await _client.SendAsync(request);
        _testOutputHelper.WriteLine($"Status Code: {response.StatusCode}");
        var content = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Response Content: {content}");
        Assert.True(response.IsSuccessStatusCode);

        var countriesList = JsonSerializer.Deserialize<List<App.DTO.v1_0.Country>>(content, _camelCaseJsonSerializerOptions)!;
        Assert.NotEmpty(countriesList);
    }

    [Fact(DisplayName = "GET - get country by ID")]
    public async Task GetCountryById()
    {
        var responseContent = await LoginAndCreateUser();
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;

        var country = await CreateCountry(jwtData);

        var url = $"/api/v1/Countries/{country.Id}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);

        var response = await _client.SendAsync(request);
        _testOutputHelper.WriteLine($"Status Code: {response.StatusCode}");
        var content = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Response Content: {content}");
        Assert.True(response.IsSuccessStatusCode);

        var retrievedCountry = JsonSerializer.Deserialize<App.DTO.v1_0.Country>(content, _camelCaseJsonSerializerOptions)!;
        Assert.Equal(country.Id, retrievedCountry.Id);
        Assert.Equal(country.Name, retrievedCountry.Name);
    }

    [Fact(DisplayName = "POST - add country")]
    public async Task AddCountry()
    {
        var responseContent = await LoginAndCreateUser();
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;

        var url = "/api/v1/Countries";
        var country = new App.DTO.v1_0.Country
        {
            Name = "Test Country"
        };
        var data = JsonContent.Create(country);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
        var response = await _client.PostAsync(url, data);
        _testOutputHelper.WriteLine($"Status Code: {response.StatusCode}");
        var content = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Response Content: {content}");
        Assert.True(response.IsSuccessStatusCode);

        var createdCountry = JsonSerializer.Deserialize<App.DTO.v1_0.Country>(content, _camelCaseJsonSerializerOptions)!;
        Assert.NotEqual(Guid.Empty, createdCountry.Id);
        Assert.Equal(country.Name, createdCountry.Name);
    }

    [Fact(DisplayName = "PUT - update country")]
    public async Task UpdateCountry()
    {
        var responseContent = await LoginAndCreateUser();
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;

        var country = await CreateCountry(jwtData);

        var url = $"/api/v1/Countries/{country.Id}";
        var updatedCountry = new App.DTO.v1_0.Country
        {
            Id = country.Id,
            Name = "Updated Country"
        };
        var data = JsonContent.Create(updatedCountry);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
        var response = await _client.PutAsync(url, data);
        _testOutputHelper.WriteLine($"Status Code: {response.StatusCode}");
        var content = await response.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Response Content: {content}");
        Assert.True(response.IsSuccessStatusCode);

        var getUrl = $"/api/v1/Countries/{country.Id}";
        var getRequest = new HttpRequestMessage(HttpMethod.Get, getUrl);
        getRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);

        var getResponse = await _client.SendAsync(getRequest);
        _testOutputHelper.WriteLine($"Status Code: {getResponse.StatusCode}");
        var getContent = await getResponse.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Response Content: {getContent}");
        Assert.True(getResponse.IsSuccessStatusCode);

        var retrievedCountry = JsonSerializer.Deserialize<App.DTO.v1_0.Country>(getContent, _camelCaseJsonSerializerOptions)!;
        Assert.Equal(updatedCountry.Name, retrievedCountry.Name);
    }

    [Fact(DisplayName = "DELETE - delete country")]
    public async Task DeleteCountry()
    {
        var responseContent = await LoginAndCreateUser();
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;

        var country = await CreateCountry(jwtData);

        // Verify that the country exists before attempting to delete
        var checkUrl = $"/api/v1/Countries/{country.Id}";
        var checkRequest = new HttpRequestMessage(HttpMethod.Get, checkUrl);
        checkRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
        var checkResponse = await _client.SendAsync(checkRequest);
        _testOutputHelper.WriteLine($"Check Status Code: {checkResponse.StatusCode}");
        var checkContent = await checkResponse.Content.ReadAsStringAsync();
        _testOutputHelper.WriteLine($"Check Response Content: {checkContent}");

        var url = $"/api/v1/Countries/{country.Id}";
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
        var response = await _client.DeleteAsync(url);
        _testOutputHelper.WriteLine($"Delete Status Code: {response.StatusCode}");
        Assert.True(response.IsSuccessStatusCode);

        var getUrl = $"/api/v1/Countries/{country.Id}";
        var getRequest = new HttpRequestMessage(HttpMethod.Get, getUrl);
        getRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
        var getResponse = await _client.SendAsync(getRequest);
        _testOutputHelper.WriteLine($"After Delete Status Code: {getResponse.StatusCode}");
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
    
    [Fact]
    public async Task PostCountryTest()
    {
        // Arrange
        var newCountry = new { Name = "Testlandia" };
        var content = JsonContent.Create(newCountry);

        // Act
        var postResponse = await _client.PostAsync("/api/countries", content);

        // Assert that the country was created successfully
        Assert.Equal(HttpStatusCode.Created, postResponse.StatusCode);
        var postContent = await postResponse.Content.ReadAsStringAsync();
        var createdCountry = JsonSerializer.Deserialize<dynamic>(postContent);
        Assert.NotNull(createdCountry);
        Assert.NotEqual(Guid.Empty, createdCountry!.Id);

        _testOutputHelper.WriteLine($"Created Country ID: {createdCountry.Id}");

        // Verify the country exists
        var verifyResponse = await _client.GetAsync($"/api/countries/{createdCountry.Id}");
        Assert.Equal(HttpStatusCode.OK, verifyResponse.StatusCode);
        var verifyContent = await verifyResponse.Content.ReadAsStringAsync();
        var verifiedCountry = JsonSerializer.Deserialize<dynamic>(verifyContent);
        Assert.Equal("Testlandia", verifiedCountry!.Name);
        Assert.Equal("TST", verifiedCountry.IsoCode);

        // Optionally, clean up by deleting the country
        var deleteResponse = await _client.DeleteAsync($"/api/countries/{createdCountry.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // Optionally, verify cleanup
        var checkDeleteResponse = await _client.GetAsync($"/api/countries/{createdCountry.Id}");
        Assert.Equal(HttpStatusCode.NotFound, checkDeleteResponse.StatusCode);
    }


    private async Task<App.DTO.v1_0.Country> CreateCountry(JWTResponse jwtData)
    {
        var url = "/api/v1/Countries";
        var country = new App.DTO.v1_0.Country
        {
            Name = "Test Country"
        };
        var data = JsonContent.Create(country);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
        var response = await _client.PostAsync(url, data);
        var content = await response.Content.ReadAsStringAsync();
        var createdCountry = JsonSerializer.Deserialize<App.DTO.v1_0.Country>(content, _camelCaseJsonSerializerOptions)!;

        return createdCountry;
    }
}
