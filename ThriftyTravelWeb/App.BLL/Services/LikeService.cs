using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class LikeService :
    BaseEntityService<App.DAL.DTO.Like, App.BLL.DTO.Like, ILikeRepository>,
    ILikeService
{
    public LikeService(IAppUnitOfWork uoW, ILikeRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Like, App.BLL.DTO.Like>(mapper))
    {
    }
}