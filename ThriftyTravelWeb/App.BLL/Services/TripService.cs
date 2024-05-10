using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class TripService :
    BaseEntityService<App.DAL.DTO.Trip, App.BLL.DTO.Trip, ITripRepository>,
    ITripService
{
    public TripService(IAppUnitOfWork uoW, ITripRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Trip, App.BLL.DTO.Trip>(mapper))
    {
    }

}