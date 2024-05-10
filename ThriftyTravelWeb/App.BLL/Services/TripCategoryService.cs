using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class TripCategoryService :
    BaseEntityService<App.DAL.DTO.TripCategory, App.BLL.DTO.TripCategory, ITripCategoryRepository>,
    ITripCategoryService
{
    public TripCategoryService(IAppUnitOfWork uoW, ITripCategoryRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.TripCategory, App.BLL.DTO.TripCategory>(mapper))
    {
    }
}