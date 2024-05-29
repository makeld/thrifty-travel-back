using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IExpenseRepository : IEntityRepository<App.DAL.DTO.Expense>, IExpenseRepositoryCustom<App.DAL.DTO.Expense>
{
    
}

public interface IExpenseRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetExpenseByTripId(Guid tripId);
    
}