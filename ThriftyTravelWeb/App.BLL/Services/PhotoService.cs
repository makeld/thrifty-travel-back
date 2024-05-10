using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class PhotoService :
    BaseEntityService<App.DAL.DTO.Photo, App.BLL.DTO.Photo, IPhotoRepository>,
    IPhotoService
{
    public PhotoService(IAppUnitOfWork uoW, IPhotoRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Photo, App.BLL.DTO.Photo>(mapper))
    {
    }
}