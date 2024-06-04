using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using App.BLL.DTO;
using App.DTO.v1_0.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
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
        
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions);
        
        // Act
        var country = JsonSerializer.Deserialize<Country>(await AddCountry(jwtData!), _camelCaseJsonSerializerOptions)!;

        var url = "/api/v1/Countries";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);

        // Act
        var response = await _client.SendAsync(request);
        _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);

    }

    [Fact(DisplayName = "GET - get country by ID")]
    public async Task GetCountryById()
    {
        var responseContent = await LoginAndCreateUser();
        
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions);
        
        // Act
        var country = JsonSerializer.Deserialize<Country>(await AddCountry(jwtData!), _camelCaseJsonSerializerOptions)!;
        _testOutputHelper.WriteLine(country.Id.ToString());
        _testOutputHelper.WriteLine(country.Name);
        _testOutputHelper.WriteLine(country.Continent);
        

        var url = $"/api/v1/Countries/{country.Id}";
        
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);

        // Act
        var response = await _client.SendAsync(request);
        _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }

    [Fact(DisplayName = "PUT - update country")]
    public async Task UpdateCountry()
    {
        var responseContent = await LoginAndCreateUser();
        
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
        var country = JsonSerializer.Deserialize<Country>(await AddCountry(jwtData), _camelCaseJsonSerializerOptions)!;
        
        var url = $"/api/v1/Countries/{country.Id}";
        
        var data = new Country()
        {
            Id = country.Id,
            Name = "TEST2",
            Continent = "TEST2"
        };

        var content = JsonContent.Create(data);
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);

        var response = await _client.PutAsync(url, content);
        var getResponse = await _client.GetAsync(url);
        _testOutputHelper.WriteLine(await getResponse.Content.ReadAsStringAsync());
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);

    }

    [Fact(DisplayName = "DELETE - delete country")]
    public async Task DeleteCountry()
    {
        var responseContent = await LoginAndCreateUser();
        
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions);
        
        // Act
        var country = JsonSerializer.Deserialize<Country>(await AddCountry(jwtData!), _camelCaseJsonSerializerOptions)!;
        
        var url = $"/api/v1/Countries/{country.Id}";
        
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);

        var response = await _client.DeleteAsync(url);
        _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
    }
    
    [Fact(DisplayName = "POST - add Country")]
    public async Task PostCountry()
    {
        var responseContent = await LoginAndCreateUser();
        
        var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions);
        
        // Act
        var country = JsonSerializer.Deserialize<Country>(await AddCountry(jwtData!), _camelCaseJsonSerializerOptions)!;
        
        // Assert
        Assert.NotNull(country.Name);
        Assert.NotNull(country.Continent);
    }
    
    public async Task<string> AddCountry(JWTResponse jwtData)
    {
    
        var url = "/api/v1/Countries";
        var country = new App.BLL.DTO.Country()
        {
            Id = Guid.NewGuid(),
            Name = "TEST",
            Continent = "TEST"
        };
        var data =  JsonContent.Create(country);
    
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);
        
        // Act
        var response = await _client.PostAsync(url, data);
        
        // Assert
        Assert.True(response.IsSuccessStatusCode);
    
        var responseContent = await response.Content.ReadAsStringAsync();
        return responseContent;
    }

}
