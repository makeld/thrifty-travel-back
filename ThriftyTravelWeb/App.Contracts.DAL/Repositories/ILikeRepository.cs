using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ILikeRepository : IEntityRepository<App.DAL.DTO.Like>, ILikeRepositoryCustom<App.DAL.DTO.Like>
{
    
}

public interface ILikeRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllLikesByAppUserId(Guid appUserId);
    Task<IEnumerable<TEntity?>> GetAllLikesByTripId(Guid tripId);
    
}