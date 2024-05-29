using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IUserExpenseRepository : IEntityRepository<App.DAL.DTO.UserExpense>, IUserExpenseRepositoryCustom<App.DAL.DTO.UserExpense>
{
    
}

public interface IUserExpenseRepositoryCustom<TEntity>
{
    Task<IEnumerable<TEntity?>> GetAllUserExpensesByTripUserId(Guid tripUserId);
    Task<IEnumerable<TEntity?>> GetAllUserExpensesByExpenseId(Guid expenseId);
    
}