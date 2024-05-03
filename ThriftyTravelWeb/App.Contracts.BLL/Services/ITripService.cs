using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITripService : IEntityRepository<Trip>, ITripRepositoryCustom<Trip>
{
    
}