using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ITripLocationRepository : IEntityRepository<App.DAL.DTO.TripLocation>, ITripLocationRepositoryCustom<App.DAL.DTO.TripLocation>
{
    
}

public interface ITripLocationRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllTripLocationsByLocationId(Guid locationId);
    Task<IEnumerable<TEntity?>> GetAllTripLocationsByTripId(Guid tripId);
    
}