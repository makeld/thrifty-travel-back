using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IUserExpenseRepository : IEntityRepository<App.DAL.DTO.UserExpense>
{
    
}