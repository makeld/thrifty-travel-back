using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Trip = App.BLL.DTO.Trip;

namespace App.BLL.Services;

public class TripService :
    BaseEntityService<App.DAL.DTO.Trip, Trip, ITripRepository>,
    ITripService
{
    public TripService(IAppUnitOfWork uoW, ITripRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Trip, Trip>(mapper))
    {
    }

    public async Task<IEnumerable<Trip>> GetAllSortedAsync(Guid userId)
    {
        return (await Repository.GetAllSortedAsync(userId)).Select(e => Mapper.Map(e));
    }
}