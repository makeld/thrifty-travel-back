using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ICommentService : IEntityRepository<App.BLL.DTO.Comment>
{
    
}