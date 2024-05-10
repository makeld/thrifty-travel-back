using App.BLL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IExpenseService : IEntityRepository<App.BLL.DTO.Expense>
{
    
}