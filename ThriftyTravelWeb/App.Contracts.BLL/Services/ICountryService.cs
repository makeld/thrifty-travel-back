using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ICountryService : IEntityRepository<App.BLL.DTO.Country>
{
    
}