using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IPhotoService :  IEntityRepository<App.BLL.DTO.Photo>, IPhotoRepositoryCustom<App.BLL.DTO.Photo>
{

}