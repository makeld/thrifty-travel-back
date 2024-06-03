using Base.Contracts.DAL;
using Microsoft.EntityFrameworkCore;

namespace Base.Test.BLL;

public class TestBaseUnitOfWork<TAppDbContext> : IUnitOfWork 
    where TAppDbContext : DbContext
{
    protected readonly TAppDbContext UowDbContext;

    protected TestBaseUnitOfWork(TAppDbContext dbContext)
    {
        UowDbContext = dbContext;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await UowDbContext.SaveChangesAsync();
    }
    
    public int SaveChanges()
    {
        return UowDbContext.SaveChanges();
    }
}