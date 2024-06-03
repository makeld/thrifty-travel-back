using App.DAL.EF;
using AutoMapper;
using Base.Test.Domain;
using Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Base.Test.DAL;

public class TestBaseRepository
{
    private readonly ITestOutputHelper _testOutputHelper;
    private readonly TestDbContext _ctx;
    private readonly TestEntityRepository _repository;
    
    public TestBaseRepository(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
        // set up mock database - inmemory
        var optionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
        var config = new MapperConfiguration(cfg => cfg.CreateMap<TestEntity, TestEntity>());
        var mapper = config.CreateMapper();

        // use random guid as db instance id
        optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        _ctx = new TestDbContext(optionsBuilder.Options);

        // reset db
        _ctx.Database.EnsureDeleted();
        _ctx.Database.EnsureCreated();
        _repository = new TestEntityRepository(_ctx, new DalDomainMapper<TestEntity, TestEntity>(mapper));

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
        _repository.Add(entity1);
        _repository.Add(entity2);
        _ctx.SaveChanges();
        // act
        var data = (_repository.GetAll()).Where(x => x.Id == entity1.Id || x.Id == entity2.Id);
        
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
        _repository.Add(entity1);
        _repository.Add(entity2);
        await _ctx.SaveChangesAsync();
        // act
        var data = (await _repository.GetAllAsync()).Where(x => x.Id == entity1.Id || x.Id == entity2.Id);
        
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
        _repository.Add(entity);
        await _ctx.SaveChangesAsync();
        // act
        var data = (await _repository.GetAllAsync()).First(x => x.Id == entity.Id);
        
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
        
        _repository.Add(entity);
        await _ctx.SaveChangesAsync();
        
        // act
        
        Assert.True(await _repository.ExistsAsync(entity.Id));
    }

    [Fact]
    public async Task TestFirstOrDefaultAsync()
    {
        Assert.Null(await _repository.FirstOrDefaultAsync(Guid.NewGuid()));
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
        
        _repository.Add(entity);
        _ctx.SaveChangesAsync();
        
        // act
        
        Assert.True(_repository.Exists(entity.Id));
    }
    
    [Fact]
    public void TestFirstOrDefault()
    {
        Assert.Null(_repository.FirstOrDefault(Guid.NewGuid()));
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
        _repository.Add(entity);
        await _ctx.SaveChangesAsync();
        // act
        Assert.True(await _repository.ExistsAsync(entity.Id));
        // arrange
        await _repository.RemoveAsync(entity.Id, entity.AppUserId);
        await _ctx.SaveChangesAsync();
        // act
        Assert.False(await _repository.ExistsAsync(entity.Id));
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
        _repository.Add(entity);
        _ctx.SaveChanges();
        // act
        Assert.True(_repository.Exists(entity.Id));
        // arrange
        _repository.Remove(entity.Id, entity.AppUser.Id);
        _ctx.SaveChanges();
        // act
        Assert.False(_repository.Exists(entity.Id));
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
        _repository.Add(entity);
        _ctx.SaveChanges();
        // act
        Assert.True(_repository.Exists(entity.Id));
        // arrange
        _repository.Remove(entity.Id);
        _ctx.SaveChanges();
        
        // act
        Assert.False(_repository.Exists(entity.Id));
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
        _repository.Add(entity);
        _ctx.SaveChanges();
        // act
        Assert.True(_repository.Exists(entity.Id));
        // arrange
        _repository.Remove(entity);
        _ctx.SaveChanges();
        // act
        Assert.False(_repository.Exists(entity.Id));
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
        _repository.Add(entity);
        _ctx.SaveChanges();
        // act
        Assert.True(_repository.Exists(entity.Id));
        // arrange
        _repository.Remove(entity, entity.AppUserId);
        _ctx.SaveChanges();
        // act
        Assert.False(_repository.Exists(entity.Id));
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
        
        _repository.Add(entity);
        await _ctx.SaveChangesAsync();
        // act
        Assert.True(await _repository.ExistsAsync(entity.Id));
        
        // arrange
        await _repository.RemoveAsync(entity.Id);
        await _ctx.SaveChangesAsync();
        
        // act
        Assert.False(await _repository.ExistsAsync(entity.Id));
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
    
        _repository.Add(entity);
        await _ctx.SaveChangesAsync();
        // act
        Assert.True(await _repository.ExistsAsync(entity.Id));

        // arrange
        await _repository.RemoveAsync(entity, entity.AppUserId);
        await _ctx.SaveChangesAsync();
        // act
        Assert.False(await _repository.ExistsAsync(entity.Id));
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
        
        _repository.Add(entity);
        await _ctx.SaveChangesAsync();
        // act
        Assert.True(await _repository.ExistsAsync(entity.Id));
        
        // arrange
        await _repository.RemoveAsync(entity);
        await _ctx.SaveChangesAsync();
        
        // act
        Assert.False(await _repository.ExistsAsync(entity.Id));
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
        entity = _repository.Add(entity);
        _ctx.SaveChanges();
    
        Assert.True(_repository.Exists(entity.Id));

        // Arrange
        entity.Value = "Updated";
    
        // Act
        var updatedEntity = _repository.Update(entity);
        _ctx.SaveChanges();
    
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
        entity = _repository.Add(entity);
        await _ctx.SaveChangesAsync();
    
        Assert.True(await _repository.ExistsAsync(entity.Id));

        // Arrange
        entity.Value = "Updated";
    
        // Act
        var updatedEntity = _repository.Update(entity);
        await _ctx.SaveChangesAsync();
    
        // Assert
        Assert.Equal("Updated", updatedEntity.Value);
    }
}