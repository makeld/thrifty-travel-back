using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ITripCategoryRepository : IEntityRepository<App.DAL.DTO.TripCategory>, ITripCategoryRepositoryCustom<App.DAL.DTO.TripCategory>
{
    
}

public interface ITripCategoryRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllTripCategoriesByCategoryId(Guid categoryId);
    Task<IEnumerable<TEntity?>> GetAllTripCategoriesByTripId(Guid tripId);
    
}