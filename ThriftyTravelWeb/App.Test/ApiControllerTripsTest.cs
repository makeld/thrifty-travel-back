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

public class ApiControllerTripTest
{
    private readonly ITestOutputHelper _testOutputHelper;

    private readonly AppDbContext _ctx;

    private readonly IAppBLL _bll;

    private readonly IAppUnitOfWork _uow;

    private WebApp.ApiControllers.TripsController _controller;
    private readonly UserManager<AppUser> _userManager;

    public ApiControllerTripTest(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());

        var configUow = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Domain.Entities.Trip, DAL.DTO.Trip>().ReverseMap();
            cfg.CreateMap<Domain.Entities.TripUser, DAL.DTO.TripUser>().ReverseMap();
            cfg.CreateMap<Domain.Entities.Photo, DAL.DTO.Photo>().ReverseMap();
            cfg.CreateMap<Domain.Entities.Category, DAL.DTO.Category>().ReverseMap();
            cfg.CreateMap<Domain.Entities.TripCategory, DAL.DTO.TripCategory>().ReverseMap();
        });
        var mapperUow = configUow.CreateMapper();

        var configBll = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<DAL.DTO.Trip, BLL.DTO.Trip>().ReverseMap();
            cfg.CreateMap<DAL.DTO.TripUser, BLL.DTO.TripUser>().ReverseMap();
            cfg.CreateMap<DAL.DTO.Photo, BLL.DTO.Photo>().ReverseMap();
            cfg.CreateMap<DAL.DTO.Category, BLL.DTO.Category>().ReverseMap();
            cfg.CreateMap<DAL.DTO.TripCategory, BLL.DTO.TripCategory>().ReverseMap();
        });
        var mapperBll = configBll.CreateMapper();

        var configWeb = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<BLL.DTO.Trip, DTO.v1_0.Trip>().ReverseMap();
            cfg.CreateMap<BLL.DTO.AddTrip, DTO.v1_0.AddTrip>().ReverseMap();
        });
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

        _controller = new WebApp.ApiControllers.TripsController(_bll, _userManager, mapperWeb);

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
    public async Task GetTripsTest()
    {
        var result = await _controller.GetTrips();
        var okRes = result.Result as OkObjectResult;
        var val = okRes!.Value as IEnumerable<DTO.v1_0.Trip>;
        Assert.Empty(val!);
    }

    [Fact]
    public async Task GetTripTest()
    {
        var id = Guid.NewGuid();

        var trip = new BLL.DTO.Trip
        {
            Id = id,
            Title = "Existing Trip",
            Description = "Existing Description",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _bll.TripService.Add(trip);
        await _bll.SaveChangesAsync();

        var tripUser = new BLL.DTO.TripUser
        {
            Id = Guid.NewGuid(),
            TripId = id,
            AppUserId = Guid.NewGuid()
        };
        _bll.TripUserService.Add(tripUser);

        var category = new BLL.DTO.Category
        {
            Id = Guid.NewGuid(),
            Name = "Test Category"
        };
        _bll.CategoryService.Add(category);

        var tripCategory = new BLL.DTO.TripCategory
        {
            Id = Guid.NewGuid(),
            TripId = id,
            CategoryId = category.Id
        };
        _bll.TripCategoryService.Add(tripCategory);

        var photo = new BLL.DTO.Photo
        {
            Id = Guid.NewGuid(),
            TripId = id,
            ImageUrl = "http://example.com/image.jpg",
            Description = "Test Image"
        };
        _bll.PhotoService.Add(photo);

        await _bll.SaveChangesAsync();

        var result = await _controller.GetTrip(id);
        var okRes = result.Result as OkObjectResult;
        var val = okRes!.Value as DTO.v1_0.AddTrip;
        Assert.NotNull(val);
        Assert.Equal(trip.Title, val!.TripTitle);
    }

    [Fact]
    public async Task PostTripTest()
    {
        var trip = new DTO.v1_0.AddTrip
        {
            TripTitle = "Test Trip",
            TripDescription = "Test Description",
            TripCategory = "Test Category",
            ImageUrl = "http://example.com/image.jpg",
            ImageDescription = "Test Image"
        };

        var result = await _controller.PostTrip(trip);
        var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetTrip", createdAtActionResult.ActionName);
        Assert.Equal(trip.TripTitle, ((DTO.v1_0.Trip)createdAtActionResult.Value!).Title);
    }

    [Fact]
    public async Task PutTripTest()
    {
        var id = Guid.NewGuid();
        var trip = new DTO.v1_0.AddTrip
        {
            TripId = id,
            TripTitle = "Updated Trip",
            TripDescription = "Updated Description",
            TripCategory = "Updated Category",
            ImageUrl = "http://example.com/updated_image.jpg",
            ImageDescription = "Updated Image",
            TripUserId = Guid.NewGuid(),
            CategoryId = Guid.NewGuid(),
            TripCategoryId = Guid.NewGuid(),
            ImageId = Guid.NewGuid()
        };

        var bllTrip = new BLL.DTO.Trip
        {
            Id = id,
            Title = "Existing Trip",
            Description = "Existing Description",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _bll.TripService.Add(bllTrip);

        var tripUser = new BLL.DTO.TripUser
        {
            Id = trip.TripUserId.Value,
            TripId = id,
            AppUserId = Guid.NewGuid()
        };
        _bll.TripUserService.Add(tripUser);

        var category = new BLL.DTO.Category
        {
            Id = trip.CategoryId.Value,
            Name = "Test Category"
        };
        _bll.CategoryService.Add(category);

        var tripCategory = new BLL.DTO.TripCategory
        {
            Id = trip.TripCategoryId.Value,
            TripId = id,
            CategoryId = category.Id
        };
        _bll.TripCategoryService.Add(tripCategory);

        var photo = new BLL.DTO.Photo
        {
            Id = trip.ImageId.Value,
            TripId = id,
            ImageUrl = "http://example.com/image.jpg",
            Description = "Test Image"
        };
        _bll.PhotoService.Add(photo);

        await _bll.SaveChangesAsync();

        var result = await _controller.PutTrip(id, trip);
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteTripTest()
    {
        var id = Guid.NewGuid();

        var bllTrip = new BLL.DTO.Trip
        {
            Id = id,
            Title = "Trip to Delete",
            Description = "Trip Description",
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
        };
        _bll.TripService.Add(bllTrip);

        var tripUser = new BLL.DTO.TripUser
        {
            Id = Guid.NewGuid(),
            TripId = id,
            AppUserId = Guid.NewGuid()
        };
        _bll.TripUserService.Add(tripUser);

        var category = new BLL.DTO.Category
        {
            Id = Guid.NewGuid(),
            Name = "Test Category"
        };
        _bll.CategoryService.Add(category);

        var tripCategory = new BLL.DTO.TripCategory
        {
            Id = Guid.NewGuid(),
            TripId = id,
            CategoryId = category.Id
        };
        _bll.TripCategoryService.Add(tripCategory);

        var photo = new BLL.DTO.Photo
        {
            Id = Guid.NewGuid(),
            TripId = id,
            ImageUrl = "http://example.com/image.jpg",
            Description = "Test Image"
        };
        _bll.PhotoService.Add(photo);

        await _bll.SaveChangesAsync();

        var result = await _controller.DeleteTrip(id);
        Assert.IsType<NoContentResult>(result);
    }
}
