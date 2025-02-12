using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITripCategoryService : IEntityRepository<App.BLL.DTO.TripCategory>, ITripCategoryRepositoryCustom<App.BLL.DTO.TripCategory>
{
    
}