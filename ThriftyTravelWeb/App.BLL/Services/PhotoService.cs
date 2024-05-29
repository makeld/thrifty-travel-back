using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class PhotoService :
    BaseEntityService<App.DAL.DTO.Photo, App.BLL.DTO.Photo, IPhotoRepository>,
    IPhotoService
{
    public PhotoService(IAppUnitOfWork uoW, IPhotoRepository repository, IMapper mapper) : 
        base(uoW, repository, new BllDalMapper<App.DAL.DTO.Photo, App.BLL.DTO.Photo>(mapper))
    {
    }

    public async Task<IEnumerable<Photo?>> GetAllPhotosByExpenseId(Guid expenseId)
    {
        return (await Repository.GetAllPhotosByExpenseId(expenseId)).Select(e => Mapper.Map(e));
    }

    public async Task<IEnumerable<Photo?>> GetAllPhotosByTripId(Guid tripId)
    {
        return (await Repository.GetAllPhotosByTripId(tripId)).Select(e => Mapper.Map(e));
    }
}