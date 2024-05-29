using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface IExpenseService : IEntityRepository<App.BLL.DTO.Expense>, IExpenseRepositoryCustom<App.BLL.DTO.Expense>
{
    
}