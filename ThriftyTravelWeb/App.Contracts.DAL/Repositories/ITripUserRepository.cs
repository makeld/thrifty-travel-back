using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ITripUserRepository : IEntityRepository<App.DAL.DTO.TripUser>, ITripUserRepositoryCustom<App.DAL.DTO.TripUser>
{
    
}

public interface ITripUserRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllTripUsersByAppUserId(Guid appUserId);
    Task<IEnumerable<TEntity?>> GetAllTripUsersByTripId(Guid tripId);
    
}