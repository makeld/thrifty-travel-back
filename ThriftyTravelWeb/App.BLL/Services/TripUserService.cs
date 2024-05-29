using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class TripUserService :
    BaseEntityService<App.DAL.DTO.TripUser, App.BLL.DTO.TripUser, ITripUserRepository>,
    ITripUserService
{
    public TripUserService(IAppUnitOfWork uoW, ITripUserRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.TripUser, App.BLL.DTO.TripUser>(mapper))
    {
    }

    public async Task<IEnumerable<TripUser?>> GetAllTripUsersByAppUserId(Guid appUserId)
    {
        return (await Repository.GetAllTripUsersByAppUserId(appUserId)).Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<TripUser?>> GetAllTripUsersByTripId(Guid tripId)
    {
        return (await Repository.GetAllTripUsersByTripId(tripId)).Select(e => Mapper.Map(e));
    }
}