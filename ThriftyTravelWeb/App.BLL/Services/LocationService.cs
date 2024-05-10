using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class LocationService :
    BaseEntityService<App.DAL.DTO.Location, App.BLL.DTO.Location, ILocationRepository>,
    ILocationService
{
    public LocationService(IAppUnitOfWork uoW, ILocationRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Location, App.BLL.DTO.Location>(mapper))
    {
    }
}