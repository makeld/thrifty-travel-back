using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class TripUserService :
    BaseEntityService<App.DAL.DTO.TripUser, App.BLL.DTO.TripUser, ITripUserRepository>,
    ITripUserService
{
    public TripUserService(IAppUnitOfWork uoW, ITripUserRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.TripUser, App.BLL.DTO.TripUser>(mapper))
    {
    }
}