using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITripLocationService : IEntityRepository<App.BLL.DTO.TripLocation>, ITripLocationRepositoryCustom<App.BLL.DTO.TripLocation>
{
    
}