using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class CountryService :
    BaseEntityService<App.DAL.DTO.Country, App.BLL.DTO.Country, ICountryRepository>,
    ICountryService
{
    public CountryService(IAppUnitOfWork uoW, ICountryRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Country, App.BLL.DTO.Country>(mapper))
    {
    }

}