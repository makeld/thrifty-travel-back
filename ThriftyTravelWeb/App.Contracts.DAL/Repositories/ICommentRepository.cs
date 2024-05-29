using Base.Contracts.DAL;
using DALDTO = App.DAL.DTO;

namespace App.Contracts.DAL.Repositories;

public interface ICommentRepository : IEntityRepository<DALDTO.Comment>, ICommentRepositoryCustom<DALDTO.Comment>
{

}
public interface ICommentRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllCommentsByAppUserId(Guid appUserId);
    Task<IEnumerable<TEntity?>> GetAllCommentsByTripId(Guid tripId);

}