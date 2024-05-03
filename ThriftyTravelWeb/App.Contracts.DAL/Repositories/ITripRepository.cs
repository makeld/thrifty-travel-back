using Base.Contracts.DAL;
using Domain.Entities;

namespace App.Contracts.DAL.Repositories;

public interface ITripRepository : IEntityRepository<App.DAL.DTO.Trip>, ITripRepositoryCustom<App.DAL.DTO.Trip>
{
    // define your DAL only custom methods here
}

public interface ITripRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllSortedAsync(Guid userId);
}