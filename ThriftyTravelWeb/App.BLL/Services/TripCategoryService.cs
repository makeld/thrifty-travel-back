using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.DTO;
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

    public async Task<IEnumerable<App.BLL.DTO.TripCategory?>> GetAllTripCategoriesByCategoryId(Guid categoryId)
    {
        return (await Repository.GetAllTripCategoriesByCategoryId(categoryId)).Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<App.BLL.DTO.TripCategory?>> GetAllTripCategoriesByTripId(Guid tripId)
    {
        return (await Repository.GetAllTripCategoriesByCategoryId(tripId)).Select(e => Mapper.Map(e));
    }
}