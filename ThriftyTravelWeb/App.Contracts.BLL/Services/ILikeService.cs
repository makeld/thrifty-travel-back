using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ILikeService : IEntityRepository<App.BLL.DTO.Like>, ILikeRepositoryCustom<App.BLL.DTO.Like>
{
}