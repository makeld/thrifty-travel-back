using App.BLL;
using App.Contracts.BLL;
using App.Contracts.DAL;
using App.DAL.EF;
using AutoMapper;
using Domain.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using Xunit;
using Xunit.Abstractions;

namespace App.Test;

public class ApiControllerCountryTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly AppDbContext _ctx;

    private readonly IAppBLL _bll;

    private readonly IAppUnitOfWork _uow;

    private WebApp.ApiControllers.CountriesController _controller;
    private readonly UserManager<AppUser> _userManager;

    public ApiControllerCountryTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        var configUow = new MapperConfiguration(cfg =>
            cfg.CreateMap<Domain.Entities.Country, DAL.DTO.Country>().ReverseMap());
        var mapperUow = configUow.CreateMapper();

        var configBll = new MapperConfiguration(cfg =>
            cfg.CreateMap<DAL.DTO.Country, BLL.DTO.Country>().ReverseMap());
        var mapperBll = configBll.CreateMapper();

        var configWeb = new MapperConfiguration(cfg =>
            cfg.CreateMap<BLL.DTO.Country, DTO.v1_0.Country>().ReverseMap());
        var mapperWeb = configWeb.CreateMapper();

        _ctx = new AppDbContext(optionsBuilder.Options);
        _uow = new AppUOW(_ctx, mapperUow);
        _bll = new AppBLL(_uow, mapperBll);

        var storeStub = Substitute.For<IUserStore<AppUser>>();
        var optionsStub = Substitute.For<IOptions<IdentityOptions>>();
        var hasherStub = Substitute.For<IPasswordHasher<AppUser>>();

        var validatorStub = Substitute.For<IEnumerable<IUserValidator<AppUser>>>();
        var passwordStup = Substitute.For<IEnumerable<IPasswordValidator<AppUser>>>();
        var lookupStub = Substitute.For<ILookupNormalizer>();
        var errorStub = Substitute.For<IdentityErrorDescriber>();
        var serviceStub = Substitute.For<IServiceProvider>();
        var loggerStub = Substitute.For<ILogger<UserManager<AppUser>>>();

        _userManager = Substitute.For<UserManager<AppUser>>(
            storeStub,
            optionsStub,
            hasherStub,
            validatorStub, passwordStup, lookupStub, errorStub, serviceStub, loggerStub
        );

        _controller = new WebApp.ApiControllers.CountriesController(_bll, _userManager, mapperWeb);

        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging();
        var serviceProvider = serviceCollection.BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = serviceProvider
        };
        httpContext.Request.Headers["Api-Version"] = "1.0";

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        _userManager.GetUserId(_controller.User).Returns(Guid.NewGuid().ToString());
    }

    [Fact]
    public async Task GetCountriesTest()
    {
        var result = await _controller.GetCountries();
        var okRes = result.Result as OkObjectResult;
        var val = okRes!.Value as IEnumerable<DTO.v1_0.Country>;
        Assert.Empty(val!);
    }

    [Fact]
    public async Task GetCountryTest()
    {
        var id = Guid.NewGuid();
        var result = await _controller.GetCountry(id);
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task PostCountryTest()
    {
        var country = new DTO.v1_0.Country
        {
            Id = Guid.NewGuid(),
            Name = "Test Country",
            Continent = "Test Continent"
        };

        var result = await _controller.PostCountry(country);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetCountry", createdAtActionResult.ActionName);
        Assert.Equal(country.Id, ((DTO.v1_0.Country)createdAtActionResult.Value!).Id);
    }

    [Fact]
    public async Task PutCountryTest()
    {
        var id = Guid.NewGuid();
        var country = new DTO.v1_0.Country
        {
            Id = id,
            Name = "Updated Country",
            Continent = "Updated Continent"
        };

        var bllCountry = new BLL.DTO.Country
        {
            Id = id,
            Name = "Existing Country",
            Continent = "Existing Continent"
        };
        _bll.CountryService.Add(bllCountry);
        await _bll.SaveChangesAsync();

        var result = await _controller.PutCountry(id, country);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteCountryTest()
    {
        var id = Guid.NewGuid();

        var bllCountry = new BLL.DTO.Country
        {
            Id = id,
            Name = "Country to Delete",
            Continent = "Continent"
        };
        _bll.CountryService.Add(bllCountry);
        await _bll.SaveChangesAsync();

        var result = await _controller.DeleteCountry(id);
        Assert.IsType<NoContentResult>(result);
    }
}
