using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITripUserService : IEntityRepository<App.BLL.DTO.TripUser>, ITripUserRepositoryCustom<App.BLL.DTO.TripUser>
{
    
}