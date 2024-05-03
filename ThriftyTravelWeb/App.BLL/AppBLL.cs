using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.DAL.EF;
using AutoMapper;
using Base.BLL;

namespace App.BLL;

public class AppBLL: BaseBLL<AppDbContext>, IAppBLL
{
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    
    public AppBLL(IAppUnitOfWork uoW, IMapper mapper) : base(uoW)
    {
        _mapper = mapper;
        _uow = uoW;
    }

    private ITripService? _trips;
    public ITripService Trips => _trips ?? new TripService(_uow, _uow.TripRepository, _mapper);
}