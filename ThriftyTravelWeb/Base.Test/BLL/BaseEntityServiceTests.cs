using App.BLL;
using App.DAL.EF;
using AutoMapper;
using Base.Test.DAL;
using Base.Test.Domain;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Base.Test.BLL;

public class BaseEntityServiceTests
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly TestDbContext _ctx;
    private readonly TestEntityService _service;
    private readonly TestUOW _uow;

    public BaseEntityServiceTests(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        var config = new MapperConfiguration(cfg => cfg.CreateMap<TestEntity, TestEntity>());
        var mapper = config.CreateMapper();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new TestDbContext(optionsBuilder.Options);
        _uow = new TestUOW(_ctx, mapper);

        // reset db
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();
        var repository = new TestEntityRepository(_ctx, new DalDomainMapper<TestEntity, TestEntity>(mapper));
        
        _service = new TestEntityService(_uow, repository, new BllDalMapper<TestEntity, TestEntity>(mapper));
    }

    [Fact]
    public void TestGetAll()
    {
        var entity1 = new TestEntity()
        {
            Id = Guid.NewGuid(),
            Value = "foo"
        };
        
        var entity2 = new TestEntity()
        {
            Id = Guid.NewGuid(),
            Value = "bar"
        };
        
        // arrange
        _service.Add(entity1);
        _service.Add(entity2);
        _uow.SaveChanges();
        // act
        var data = (_service.GetAll()).Where(x => x.Id == entity1.Id || x.Id == entity2.Id);
        
        // assert
        
        Assert.True(data.Count() == 2);
    }
    
    [Fact]
    public async void TestGetAllAsync()
    {
        var entity1 = new TestEntity()
        {
            Id = Guid.NewGuid(),
            Value = "foo"
        };
        
        var entity2 = new TestEntity()
        {
            Id = Guid.NewGuid(),
            Value = "bar"
        };
        
        // arrange
        _service.Add(entity1);
        _service.Add(entity2);
        
        await _uow.SaveChangesAsync();
        // act
        var data = (await _service.GetAllAsync()).Where(x => x.Id == entity1.Id || x.Id == entity2.Id);
        
        // assert
        
        Assert.True(data.Count() == 2);
    }
    
    [Fact]
    public async void TestAdd()
    {
        var entity = new TestEntity()
        {
            Id = Guid.NewGuid(),
            Value = "foo"
        };
        
        // arrange
        _service.Add(entity);
        await _uow.SaveChangesAsync();

        // act
        var data = (await _service.GetAllAsync()).First(x => x.Id == entity.Id);
        
        // assert
        
        Assert.NotNull(data);
    }
    
    [Fact]
    public async void TestExistsAsync()
    {
        // arrange
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo" 
        };
        
        _service.Add(entity);
        await _uow.SaveChangesAsync();

        
        // act
        
        Assert.True(await _service.ExistsAsync(entity.Id));
    }

    [Fact]
    public async Task TestFirstOrDefaultAsync()
    {
        Assert.Null(await _service.FirstOrDefaultAsync(Guid.NewGuid()));
    }
    
    [Fact]
    public void TestExists()
    {
        // arrange
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo" 
        };
        _service.Add(entity);
        _uow.SaveChanges();
        // act
        Assert.True(_service.Exists(entity.Id));
    }
    
    [Fact]
    public void TestFirstOrDefault()
    {
        Assert.Null(_service.FirstOrDefault(Guid.NewGuid()));
    }
    
    [Fact]
    public async void TestRemoveByIdWithUserIdAsync()
    {
        var id = Guid.NewGuid();
        // arrange
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo",
            AppUserId = id,
            AppUser = new AppUser()
            {
                Id = id
            }
        };
        _service.Add(entity);
        await _uow.SaveChangesAsync();
        // act
        Assert.True(await _service.ExistsAsync(entity.Id));
        // arrange
        await _service.RemoveAsync(entity.Id, entity.AppUserId);
        await _uow.SaveChangesAsync();
        // act
        Assert.False(await _service.ExistsAsync(entity.Id));
    }
    
    [Fact]
    public void TestRemoveByIdWithUserId()
    {
        var id = Guid.NewGuid();
        // arrange
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo",
            AppUserId = id,
            AppUser = new AppUser()
            {
                Id = id
            }
        };
        _service.Add(entity);
        _uow.SaveChanges();
        // act
        Assert.True(_service.Exists(entity.Id));
        // arrange
        _service.Remove(entity.Id, entity.AppUser.Id);
        _uow.SaveChanges();
        // act
        Assert.False(_service.Exists(entity.Id));
    }
    
    [Fact]
    public void TestRemoveById()
    {
        // arrange
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo" 
        };
        _service.Add(entity);
        _uow.SaveChanges();
        // act
        Assert.True(_service.Exists(entity.Id));
        // arrange
        _service.Remove(entity.Id);
        _uow.SaveChanges();
        // act
        Assert.False(_service.Exists(entity.Id));
    }
    
    [Fact]
    public void TestRemoveByEntity()
    {
        // arrange
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo" 
        };
        _service.Add(entity);
        _uow.SaveChanges();
        // act
        Assert.True(_service.Exists(entity.Id));
        // arrange
        _service.Remove(entity);
        _uow.SaveChanges();
        // act
        Assert.False(_service.Exists(entity.Id));
    }
    
    [Fact]
    public void TestRemoveByEntityWithUserId()
    {
        var id = Guid.NewGuid();
        // arrange
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo",
            AppUserId = id,
            AppUser = new AppUser()
            {
                Id = id
            }
        };
        _service.Add(entity);
        _uow.SaveChanges();
        // act
        Assert.True(_service.Exists(entity.Id));
        // arrange
        _service.Remove(entity, entity.AppUserId);
         _uow.SaveChanges();
        // act
        Assert.False(_service.Exists(entity.Id));
    }

    
    [Fact]
    public async void TestRemoveByIdAsync()
    {
        // arrange
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo" 
        };
        _service.Add(entity);
        await _uow.SaveChangesAsync();
        // act
        Assert.True(await _service.ExistsAsync(entity.Id));
        // arrange
        await _service.RemoveAsync(entity.Id);
        await _uow.SaveChangesAsync();
        // act
        Assert.False(await _service.ExistsAsync(entity.Id));
    }
    
    [Fact]
    public async void TestRemoveByEntityWithUserIdAsync()
    {
        // arrange
        var id = Guid.NewGuid();
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo",
            AppUserId = id,
            AppUser = new AppUser()
            {
                Id = id
            }
        };
        _service.Add(entity);
        await _uow.SaveChangesAsync();
        // act
        Assert.True(await _service.ExistsAsync(entity.Id));
        // arrange
        await _service.RemoveAsync(entity, entity.AppUserId);
        await _uow.SaveChangesAsync();
        // act
        Assert.False(await _service.ExistsAsync(entity.Id));
    }

    [Fact]
    public async void TestRemoveByEntityAsync()
    {
        // arrange
        var entity = new TestEntity() 
        { 
            Id = Guid.NewGuid(),
            Value = "foo" 
        };
        _service.Add(entity);
        await _uow.SaveChangesAsync();
        // act
        Assert.True(await _service.ExistsAsync(entity.Id));
        // arrange
        await _service.RemoveAsync(entity);
        await _uow.SaveChangesAsync();
        // act
        Assert.False(await _service.ExistsAsync(entity.Id));
    }
    
    [Fact]
    public void TestUpdate()
    {
        var id = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = id,
            Value = "foo"
        };
        // arrange
        entity = _service.Add(entity);
        _uow.SaveChanges();
        Assert.True(_service.Exists(entity.Id));
        // Arrange
        entity.Value = "Updated";
        // Act
        var updatedEntity = _service.Update(entity);
        _uow.SaveChanges();
        // Assert
        Assert.Equal("Updated", updatedEntity.Value);
    }
    
    [Fact]
    public async void TestUpdateAsync()
    {
        var id = Guid.NewGuid();
        var entity = new TestEntity()
        {
            Id = id,
            Value = "foo"
        };
        // arrange
        entity = _service.Add(entity);
        await _uow.SaveChangesAsync();
        Assert.True(await _service.ExistsAsync(entity.Id));
        // Arrange
        entity.Value = "Updated";
        // Act
        var updatedEntity = _service.Update(entity);
        await _uow.SaveChangesAsync();
        // Assert
        Assert.Equal("Updated", updatedEntity.Value);
    }
}