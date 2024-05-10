using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class TripLocationService :
    BaseEntityService<App.DAL.DTO.TripLocation, App.BLL.DTO.TripLocation, ITripLocationRepository>,
    ITripLocationService
{
    public TripLocationService(IAppUnitOfWork uoW, ITripLocationRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.TripLocation, App.BLL.DTO.TripLocation>(mapper))
    {
    }
}