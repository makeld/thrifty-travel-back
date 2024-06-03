using App.BLL.DTO;
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

    public async Task<IEnumerable<TripLocation?>> GetAllTripLocationsByLocationId(Guid locationId)
    {
        return (await Repository.GetAllTripLocationsByLocationId(locationId)).Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<TripLocation?>> GetAllTripLocationsByTripId(Guid tripId)
    {
        return (await Repository.GetAllTripLocationsByTripId(tripId)).Select(e => Mapper.Map(e));
    }
}