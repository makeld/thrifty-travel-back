// using System.Net;
// using System.Net.Http.Headers;
// using System.Net.Http.Json;
// using System.Text.Json;
// using App.DTO.v1_0.Identity;
// using Microsoft.AspNetCore.Mvc.Testing;
// using Microsoft.VisualStudio.TestPlatform.TestHost;
// using Xunit.Abstractions;
// using trip = App.DTO.v1_0.Trip;
// using addTrip = App.DTO.v1_0.AddTrip;
// using Trip = App.BLL.DTO.Trip;
// using AddTrip = App.BLL.DTO.AddTrip;
//
// namespace App.Test.Integration.api;
//
// [Collection("NonParallel")]
// public class TripControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
// {
//     private readonly HttpClient _client;
//     private Guid VehicleId130Thn;
//     private readonly CustomWebApplicationFactory<Program> _factory;
//     private readonly ITestOutputHelper _testOutputHelper;
//     private readonly JsonSerializerOptions _camelCaseJsonSerializerOptions = new JsonSerializerOptions()
//     
//     
//     {
//         PropertyNamingPolicy = JsonNamingPolicy.CamelCase
//     };
//
//     public TripControllerTest(CustomWebApplicationFactory<Program> factory, ITestOutputHelper testOutputHelper)
//     {
//         _factory = factory;
//         _testOutputHelper = testOutputHelper;
//         _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
//         {
//             AllowAutoRedirect = false
//         });
//     }
//
//     private async Task<string> LoginAndCreateUser()
//     {
//         // Arrange
//         const string url = "/api/v1/identity/account/register?expiresInSeconds=123";
//         var email = $"{Guid.NewGuid()}@test.ee";
//         const string password = "Foo123Bar!";
//
//         var register = new RegisterInfo()
//         {
//             Email = email,
//             Password = password,
//         };
//         var data = JsonContent.Create(register);
//
//         // Act
//         var response = await _client.PostAsync(url, data);
//
//         // Assert
//         var responseContent = await response.Content.ReadAsStringAsync();
//         Assert.True(response.IsSuccessStatusCode);
//
//         return responseContent;
//     }
//
//     [Fact]
//     public async Task MainPageRequiresLogin()
//     {
//         // Act
//         var response = await _client.GetAsync("/api/v1/VehicleViolation/GetVehicleViolations");
//         // Assert
//         Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
//     }
//
//     [Fact(DisplayName = "POST - add VehicleViolation")]
//     public async Task AddVehicleViolation()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions);
//         // Act
//         var VehicleViolation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//         
//         // Assert
//         Assert.NotNull(VehicleViolation.Coordinates);
//         Assert.NotNull(VehicleViolation.Description);
//         Assert.NotNull(VehicleViolation.LocationName);
//         Assert.NotEqual(default, VehicleViolation.CreatedAt);
//     }
//     
//     [Fact(DisplayName = "GET - get all VehicleViolations")]
//     public async Task GetAllVehicleViolations()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var violation_130THN = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolationFor130THN(jwtData!), _camelCaseJsonSerializerOptions)!;
//         
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//
//         var url = $"/api/v1/VehicleViolation/GetVehicleViolations";
//         
//         var request = new HttpRequestMessage(HttpMethod.Get, url);
//         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
//
//         // Act
//         var response = await _client.SendAsync(request);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//         
//         var content = await response.Content.ReadAsStringAsync();
//         var vehicleViolationsList = JsonSerializer.Deserialize<List<VehicleViolation>>(content, _camelCaseJsonSerializerOptions)!;
//         
//         // 2 + 1 from previous test.
//         Assert.Equal(3, vehicleViolationsList.Count);
//         // Assert
//         foreach (var entry in vehicleViolationsList)
//         {
//             Assert.NotNull(entry.Description);
//             Assert.NotNull(entry.Coordinates);
//             Assert.NotNull(entry.LocationName);
//         }
//     }
//     
//     
//     
//     [Fact(DisplayName = "GET - get VehicleViolation by VehicleViolationId")]
//     public async Task GetVehicleViolationByVehicleViolationId()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var violation_130THN = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolationFor130THN(jwtData!), _camelCaseJsonSerializerOptions)!;
//         
//         
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//
//        
//         var url = $"/api/v1/VehicleViolation/{violation_130THN.Id}";
//         
//         var request = new HttpRequestMessage(HttpMethod.Get, url);
//         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
//
//         // Act
//         var response = await _client.SendAsync(request);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//         
//         var content = await response.Content.ReadAsStringAsync();
//         var VehicleViolation = JsonSerializer.Deserialize<VehicleViolation>(content, _camelCaseJsonSerializerOptions)!;
//         
//         // Assert
//         Assert.Equal(VehicleViolation.Id, violation_130THN.Id);
//         
//         Assert.NotNull(VehicleViolation.Description);
//         Assert.NotNull(VehicleViolation.Coordinates);
//         Assert.NotNull(VehicleViolation.LocationName);
//     }
//     
//      [Fact(DisplayName = "GET - get VehicleViolations created by current user")]
//     public async Task GetVehicleViolationsForUser()
//     {
//         //           User 1
//         var responseContent = await LoginAndCreateUser();
//         //           User 2
//         var responseContent2 = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var jwtData2 = JsonSerializer.Deserialize<JWTResponse>(responseContent2, _camelCaseJsonSerializerOptions)!;
//         
//         var violation_130THN = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolationFor130THN(jwtData!), _camelCaseJsonSerializerOptions)!;
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//         
//         var violation_130THN2 = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolationFor130THN(jwtData2!), _camelCaseJsonSerializerOptions)!;
//         var violation2 = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData2!), _camelCaseJsonSerializerOptions)!;
//
//         var url = "/api/v1/VehicleViolation/GetVehicleViolationsForUser";
//         
//         //////////////////////////////
//         //           User 1
//         //////////////////////////////
//         var request1 = new HttpRequestMessage(HttpMethod.Get, url);
//         request1.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
//         // Act
//         var response1 = await _client.SendAsync(request1);
//         // Assert
//         Assert.True(response1.IsSuccessStatusCode);
//         var content1 = await response1.Content.ReadAsStringAsync();
//         var vehicleViolationsList1 = JsonSerializer.Deserialize<List<VehicleViolation>>(content1, _camelCaseJsonSerializerOptions)!;
//         // Assert
//         foreach (var entry in vehicleViolationsList1)
//         {
//             Assert.Equal(entry.AppUserId, violation_130THN.AppUserId);
//             Assert.Equal(entry.AppUserId, violation.AppUserId);
//         }
//         
//         //////////////////////////////
//         //           User 2
//         //////////////////////////////
//         var request2 = new HttpRequestMessage(HttpMethod.Get, url);
//         request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData2.Jwt);
//         // Act
//         var response2 = await _client.SendAsync(request2);
//         // Assert
//         Assert.True(response2.IsSuccessStatusCode);
//         var content2 = await response2.Content.ReadAsStringAsync();
//         var vehicleViolationsList2 = JsonSerializer.Deserialize<List<VehicleViolation>>(content2, _camelCaseJsonSerializerOptions)!;
//         // Assert
//         foreach (var entry in vehicleViolationsList2)
//         {
//             Assert.Equal(entry.AppUserId, violation_130THN2.AppUserId);
//             Assert.Equal(entry.AppUserId, violation2.AppUserId);
//         }
//     }
//     
//     
//     [Fact(DisplayName = "GET - get VehicleViolations for a specific license plate")]
//     public async Task GetVehicleViolationsForSpecificLicensePlate()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var violation_130THN = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolationFor130THN(jwtData!), _camelCaseJsonSerializerOptions)!;
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//
//         // Using upper and lower case letters mixed in license plate url.
//         var url = "/api/v1/VehicleViolation/GetVehicleViolationsByLicensePlate/130ThN";
//         
//         var request = new HttpRequestMessage(HttpMethod.Get, url);
//         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
//
//         // Act
//         var response = await _client.SendAsync(request);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//         
//         var content = await response.Content.ReadAsStringAsync();
//         var vehicleViolationsList = JsonSerializer.Deserialize<List<VehicleViolation>>(content, _camelCaseJsonSerializerOptions)!;
//         
//         // Assert
//         foreach (var entry in vehicleViolationsList)
//         {
//             var vehicleResponse = await _client.GetAsync($"/api/v1/Vehicle/{entry.VehicleId}");
//             Assert.True(vehicleResponse.IsSuccessStatusCode);
//         
//             var vehicleContent = await vehicleResponse.Content.ReadAsStringAsync();
//             var vehicle = JsonSerializer.Deserialize<Vehicle>(vehicleContent, _camelCaseJsonSerializerOptions)!;
//
//             Assert.Equal("130THN", vehicle.RegNr);
//             Assert.True(vehicle.Rating < 5, "Vehicle rating should drop below 5 after a violation is added.");
//             
//             Assert.NotNull(entry.Description);
//             Assert.NotNull(entry.Coordinates);
//             Assert.NotNull(entry.LocationName);
//         }
//     }
//     
//     
//     [Fact(DisplayName = "GET - get the VehicleViolations by vehicle id")]
//     public async Task GetVehicleViolationWithCorrectVehicleId()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//         var violation130thn = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolationFor130THN(jwtData!), _camelCaseJsonSerializerOptions)!;
//
//         var url = $"/api/v1/VehicleViolation/GetAllVehicleViolationsByVehicleId/{VehicleId130Thn}";
//         
//         var request = new HttpRequestMessage(HttpMethod.Get, url);
//         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
//
//         // Act
//         var response = await _client.SendAsync(request);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//         
//         var content = await response.Content.ReadAsStringAsync();
//         var vehicleViolation = JsonSerializer.Deserialize<List<VehicleViolation>>(content, _camelCaseJsonSerializerOptions)![0];
//         
//         // Assert
//         Assert.Equal(VehicleId130Thn, vehicleViolation.VehicleId);
//     }
//     
//     [Fact(DisplayName = "GET - get the VehicleViolations by wrong vehicle id")]
//     public async Task GetVehicleViolationByWrongVehicleId()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//         var violation130thn = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolationFor130THN(jwtData!), _camelCaseJsonSerializerOptions)!;
//
//         var url = $"/api/v1/VehicleViolation/GetAllVehicleViolationsByVehicleId/{Guid.NewGuid()}";
//         
//         var request = new HttpRequestMessage(HttpMethod.Get, url);
//         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
//
//         // Act
//         var response = await _client.SendAsync(request);
//         
//         // Assert
//         Assert.False(response.IsSuccessStatusCode);
//     }
//     
//     [Fact(DisplayName = "GET - get VehicleViolation with wrong VehicleViolatrionId")]
//     public async Task GetVehicleViolationWithWrongVehicleViolationId()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//         
//         var url = $"/api/v1/VehicleViolation/{Guid.NewGuid()}";
//         
//         var request = new HttpRequestMessage(HttpMethod.Get, url);
//         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
//
//         // Act
//         var response = await _client.SendAsync(request);
//         
//         // Assert
//         Assert.False(response.IsSuccessStatusCode);
//     }
//     
//     [Fact(DisplayName = "PUT - update VehicleViolation with wrong VehicleViolation Id")]
//     public async Task PutVehicleViolationWrongId()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//         
//         var url = $"/api/v1/VehicleViolation/put/{violation.Id}";
//         
//         var data = new VehicleViolation()
//         {
//             Id = Guid.NewGuid(),
//             // VehicleId = vehicleId,
//             //AppUserId = AppUserId,
//             Description = "Ran a stop sign",
//             Coordinates = "59.39491;24.67178",
//             LocationName = "Tallinn",
//             CreatedAt = DateTime.Now.AddHours(-4)
//         };
//
//         var content = JsonContent.Create(data);
//         
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);
//
//         var response = await _client.PutAsync(url, content);
//         
//         // Assert
//         Assert.False(response.IsSuccessStatusCode);
//     }
//     
//     [Fact(DisplayName = "PUT - update VehicleViolation")]
//     public async Task PutVehicleViolation()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//         
//         var url = $"/api/v1/VehicleViolation/put/{violation.Id}";
//         
//         var data = new VehicleViolation()
//         {
//             Id = violation.Id,
//             ViolationId = violation.ViolationId,
//             VehicleId = violation.VehicleId,
//             AppUserId = violation.AppUserId,
//             Description = "Failure to yield",
//             Coordinates = "59.39491;24.67178",
//             LocationName = "Tallinn",
//             CreatedAt = DateTime.Now.AddHours(-4)
//         };
//
//         var content = JsonContent.Create(data);
//         
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);
//
//         var response = await _client.PutAsync(url, content);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//         
//         url = "/api/v1/VehicleViolation/GetVehicleViolations";
//         
//         var request = new HttpRequestMessage(HttpMethod.Get, url);
//         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
//
//         // Act
//         response = await _client.SendAsync(request);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//         
//         var result = await response.Content.ReadAsStringAsync();
//         var vehicleViolationsList = JsonSerializer.Deserialize<List<VehicleViolation>>(result, _camelCaseJsonSerializerOptions)!;
//         
//         // Assert
//         foreach (var entry in vehicleViolationsList)
//         {
//             if (entry.Id.Equals(violation.Id))
//             {
//                 Assert.Equal("Failure to yield", entry.Description);
//                 Assert.NotNull(entry.Coordinates);
//                 Assert.NotNull(entry.LocationName);
//                 break;
//             }
//         }
//         
//     }
//
//     [Fact(DisplayName = "DELETE - delete VehicleViolation")]
//     public async Task DeleteVehicleViolation()
//     {
//         var responseContent = await LoginAndCreateUser();
//         
//         var jwtData = JsonSerializer.Deserialize<JWTResponse>(responseContent, _camelCaseJsonSerializerOptions)!;
//         var vehicleViolationId = Guid.NewGuid();
//         
//         var violation = JsonSerializer.Deserialize<VehicleViolation>(await CreateVehicleViolation(jwtData!), _camelCaseJsonSerializerOptions)!;
//         
//         var url = $"/api/v1/VehicleViolation/delete/{violation.Id}";
//         
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);
//
//         var response = await _client.DeleteAsync(url);
//         _testOutputHelper.WriteLine(await response.Content.ReadAsStringAsync());
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//         
//         url = "/api/v1/VehicleViolation/GetVehicleViolations";
//         
//         var request = new HttpRequestMessage(HttpMethod.Get, url);
//         request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtData.Jwt);
//
//         // Act
//         response = await _client.SendAsync(request);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//         
//         var result = await response.Content.ReadAsStringAsync();
//         var VehicleViolations = JsonSerializer.Deserialize<List<VehicleViolation>>(result, _camelCaseJsonSerializerOptions)!;
//         
//         // Assert
//         foreach (var VehicleViolation in VehicleViolations)
//         {
//             Assert.NotEqual(vehicleViolationId, VehicleViolation.Id);
//         }
//     }
//     
//     public async Task<string> CreateVehicleViolation(JWTResponse jwtData)
//     {
//         var vehicleId = JsonSerializer.Deserialize<App.BLL.DTO.Vehicle>(await AddVehicle(jwtData!), _camelCaseJsonSerializerOptions)!.Id;
//         var violationId = JsonSerializer.Deserialize<App.BLL.DTO.Violation>(await AddViolation(jwtData!), _camelCaseJsonSerializerOptions)!.Id;
//         
//         var AppUserId = jwtData!.Id;
//
//         
//         var url = "/api/v1/VehicleViolation/post";
//         
//         
//         var VehicleViolation = new VehicleViolation()
//         {
//             ViolationId = violationId,
//             VehicleId = vehicleId,
//             AppUserId = Guid.Parse(AppUserId!),
//             Description = "Ran a red light",
//             Coordinates = "59.39491;24.67178",
//             LocationName = "Tallinn",
//             CreatedAt = DateTime.Now.AddHours(-4)
//         };
//         
//         var data =  JsonContent.Create(VehicleViolation);
//
//         // Act
//         var response = await _client.PostAsync(url, data);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//     
//         var content = await response.Content.ReadAsStringAsync();
//         return content;
//     }
//     
//     public async Task<string> CreateVehicleViolationFor130THN(JWTResponse jwtData)
//     {
//         var vehicleId = JsonSerializer.Deserialize<App.BLL.DTO.Vehicle>(await AddVehicle130THN(jwtData!), _camelCaseJsonSerializerOptions)!.Id;
//         VehicleId130Thn = vehicleId;
//         
//         var violationId = JsonSerializer.Deserialize<App.BLL.DTO.Violation>(await AddViolation(jwtData!), _camelCaseJsonSerializerOptions)!.Id;
//         
//         var AppUserId = jwtData!.Id;
//
//         
//         var url = "/api/v1/VehicleViolation/post";
//         
//         
//         var VehicleViolation = new VehicleViolation()
//         {
//             //Id = Guid.NewGuid(),
//             ViolationId = violationId,
//             VehicleId = vehicleId,
//             AppUserId = Guid.Parse(AppUserId!),
//             Description = "Cut off a bus",
//             Coordinates = "59.39491;24.67178",
//             LocationName = "Tallinn",
//             CreatedAt = DateTime.Now.AddHours(-4)
//         };
//         
//         var data =  JsonContent.Create(VehicleViolation);
//
//         // Act
//         var response = await _client.PostAsync(url, data);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//     
//         var content = await response.Content.ReadAsStringAsync();
//         return content;
//     }
//     
//
//     private async Task<string> AddVehicle(JWTResponse jwtData)
//     {
//         var vehicleTypeId = JsonSerializer.Deserialize<App.BLL.DTO.Vehicle>(await AddVehicleType(jwtData!), _camelCaseJsonSerializerOptions)!.Id;
//         
//         var url = "/api/v1/Vehicle/post?test=true";
//         var vehicle = new Vehicle()
//         {
//             VehicleTypeId = vehicleTypeId,
//             Color = "Blue",
//             RegNr = "640TDO"
//         };
//         var data =  JsonContent.Create(vehicle);
//
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);
//         
//         // Act
//         var response = await _client.PostAsync(url, data);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//         
//
//         var responseContent = await response.Content.ReadAsStringAsync();
//         return responseContent;
//     }
//     
//     private async Task<string> AddVehicle130THN(JWTResponse jwtData)
//     {
//         var vehicleTypeId = JsonSerializer.Deserialize<App.BLL.DTO.Vehicle>(await AddVehicleType(jwtData!), _camelCaseJsonSerializerOptions)!.Id;
//
//         
//         var url = "/api/v1/Vehicle/post?test=true";
//         var vehicle = new Vehicle()
//         {
//             VehicleTypeId = vehicleTypeId,
//             Color = "Grey",
//             RegNr = "130THN"
//         };
//         var data =  JsonContent.Create(vehicle);
//
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);
//         
//         // Act
//         var response = await _client.PostAsync(url, data);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//
//         var responseContent = await response.Content.ReadAsStringAsync();
//         return responseContent;
//     }
//
//     private async Task<string> AddViolation(JWTResponse jwtData)
//     {
//         var url = "/api/v1/Violation/post";
//         var violation = new App.BLL.DTO.Violation()
//         {
//             ViolationType = (int) EViolationType.Minor,
//             ViolationName = "Traffic violation",
//             Severity = 0
//         };
//         var data =  JsonContent.Create(violation);
//
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);
//         
//         // Act
//         var response = await _client.PostAsync(url, data);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//
//         var responseContent = await response.Content.ReadAsStringAsync();
//         return responseContent;
//     }
//
//     public async Task<string> AddVehicleType(JWTResponse jwtData)
//     {
//     
//         var url = "/api/v1/VehicleType/post";
//         var vehicleType = new App.BLL.DTO.VehicleType()
//         {
//             VehicleTypeName = "Sedan",
//             Size = (int) EVehicleSize.Sedan,
//             Make = "Honda",
//             Model = "Accord"
//         };
//         var data =  JsonContent.Create(vehicleType);
//     
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtData!.Jwt);
//         
//         // Act
//         var response = await _client.PostAsync(url, data);
//         
//         // Assert
//         Assert.True(response.IsSuccessStatusCode);
//     
//         var responseContent = await response.Content.ReadAsStringAsync();
//         return responseContent;
//     }
// }