using Base.Contracts.DAL;
using DALDTO = App.DAL.DTO;

namespace App.Contracts.DAL.Repositories;

public interface IPhotoRepository : IEntityRepository<DALDTO.Photo>, IPhotoRepositoryCustom<DALDTO.Photo>
{

}
public interface IPhotoRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllPhotosByExpenseId(Guid expenseId);
    Task<IEnumerable<TEntity?>> GetAllPhotosByTripId(Guid tripId);

}