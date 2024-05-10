using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IUserExpenseService : IEntityRepository<App.BLL.DTO.UserExpense>
{
    
}