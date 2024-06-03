using Base.Contracts.DAL;
using Base.Contracts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Base.DAL.EF;

public class BaseEntityRepository<TDomainEntity, TDalEntity, TDbContext> :
    BaseEntityRepository<Guid, TDomainEntity, TDalEntity, TDbContext>, IEntityRepository<TDalEntity>
    where TDomainEntity : class, IDomainEntityId
    where TDalEntity : class, IDomainEntityId
    where TDbContext : DbContext
{
    public BaseEntityRepository(TDbContext dbContext, IDalMapper<TDomainEntity, TDalEntity> mapper) : base(dbContext,
        mapper)
    {
    }


}

public class BaseEntityRepository<TKey, TDomainEntity, TDalEntity, TDbContext>
    where TKey : IEquatable<TKey>
    where TDomainEntity : class, IDomainEntityId
    where TDalEntity : class, IDomainEntityId
    where TDbContext : DbContext

{
    protected readonly TDbContext RepoDbContext;
    protected readonly DbSet<TDomainEntity> RepoDbSet;
    protected readonly IDalMapper<TDomainEntity, TDalEntity> Mapper;

    public BaseEntityRepository(TDbContext dbContext, IDalMapper<TDomainEntity, TDalEntity> mapper)
    {
        RepoDbContext = dbContext;
        RepoDbSet = RepoDbContext.Set<TDomainEntity>();
        Mapper = mapper;
    }

    public virtual IQueryable<TDomainEntity> CreateQuery(TKey? userId = default, bool noTracking = true)
    {
        var query = RepoDbSet.AsQueryable();
        if (userId != null && !userId.Equals(default) &&
            typeof(IDomainAppUserId<TKey>).IsAssignableFrom(typeof(TDomainEntity)))
        {
            query = query
                .Include("AppUser")
                .Where(e => ((IDomainAppUserId<TKey>) e).AppUserId.Equals(userId));
        }

        if (noTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }

    public virtual TDalEntity Add(TDalEntity entity)
    {
        return Mapper.Map(RepoDbSet.Add(Mapper.Map(entity)!).Entity)!;
    }

    public virtual TDalEntity Update(TDalEntity entity)
    {
        var entityToUpdate = RepoDbSet.Find(entity.Id);
        if (entityToUpdate != null)
        {
            RepoDbContext.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            RepoDbSet.Update(entityToUpdate);
        }

        return Mapper.Map(entityToUpdate)!;
    }

    public virtual int Remove(TDalEntity entity, TKey? userId = default)
    {
        if (userId == null || userId.Equals(Guid.Empty))
        {
            var entityToDelete = RepoDbSet.Find(entity.Id);
            RepoDbSet.Remove(entityToDelete!);
        }
        var entityToDel = CreateQuery(userId)
            .FirstOrDefault(e => e.Id.Equals(entity.Id));
        
        var trackedEntity = RepoDbSet.Find(entityToDel!.Id);
        RepoDbSet.Remove(trackedEntity!);
        return RepoDbContext.SaveChanges();
    }
    
    public virtual int Remove(TKey id, TKey? userId = default!)
    {
        if (userId == null || userId!.Equals(new Guid()))
        {
            var entity = RepoDbSet.Find(id);
            RepoDbSet.Remove(entity!);
            return RepoDbContext.SaveChanges();
        }
        var entityToDelete = CreateQuery(userId)
            .FirstOrDefault(e => e.Id.Equals(id));
        
        var trackedEntity = RepoDbSet.Find(entityToDelete!.Id);
        RepoDbSet.Remove(trackedEntity!);
        return RepoDbContext.SaveChanges();
    }


    public virtual IEnumerable<TDalEntity> GetAll(TKey userId = default!, bool noTracking = true)
    {
        return CreateQuery(userId, noTracking).ToList().Select(de => Mapper.Map(de))!;
    }

    public virtual bool Exists(TKey id, TKey userId = default!)
    {
        return CreateQuery(userId).Any(e => e.Id.Equals(id));
    }


    public virtual async Task<IEnumerable<TDalEntity>> GetAllAsync(TKey userId = default!, bool noTracking = true)
    {
        return (await CreateQuery(userId, noTracking).ToListAsync())
            .Select(de => Mapper.Map(de))!;
    }

    public virtual async Task<bool> ExistsAsync(TKey id, TKey userId = default!)
    {
        return await CreateQuery(userId).AnyAsync(e => e.Id.Equals(id));
    }

    public virtual async Task<int> RemoveAsync(TDalEntity entity, TKey? userId = default)
    {
        if (userId == null || userId!.Equals(new Guid()))
        {
            var entityToUpdate = await RepoDbSet.FindAsync(entity.Id);
            if (entityToUpdate != null)
            {
                RepoDbSet.Remove(entityToUpdate);
            }
            return await RepoDbContext.SaveChangesAsync();
        }
        var entityToDel = CreateQuery(userId)
            .FirstOrDefault(e => e.Id.Equals(entity.Id));
        
        var trackedEntity = await RepoDbSet.FindAsync(entityToDel!.Id);
        RepoDbSet.Remove(trackedEntity!);
        return await RepoDbContext.SaveChangesAsync();
    }

    public virtual async Task<int> RemoveAsync(TKey id, TKey? userId = default)
    {
        if (userId == null || userId!.Equals(new Guid()))
        {
            var entityToUpdate = await RepoDbSet.FindAsync(id);
            RepoDbSet.Remove(entityToUpdate!);
            return await RepoDbContext.SaveChangesAsync();
        }
        var entityToDel = CreateQuery(userId)
            .FirstOrDefault(e => e.Id.Equals(id));
        var trackedEntity = await RepoDbSet.FindAsync(entityToDel!.Id);
        RepoDbSet.Remove(trackedEntity!);
        return await RepoDbContext.SaveChangesAsync();
    }
    
    public TDalEntity? FirstOrDefault(TKey id, TKey userId = default!, bool noTracking = true)
    {
        return Mapper.Map(CreateQuery(userId, noTracking).FirstOrDefault());
    }

    public async Task<TDalEntity?> FirstOrDefaultAsync(TKey id, TKey userId = default!, bool noTracking = true)
    {
        var query = CreateQuery(userId, noTracking);

        query = query.Where(entity => entity.Id.Equals(id));

        return Mapper.Map(await query.FirstOrDefaultAsync());    
    }
}
