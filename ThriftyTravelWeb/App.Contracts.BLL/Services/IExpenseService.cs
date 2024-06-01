using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;
using Expense = App.DAL.DTO.Expense;

namespace App.Contracts.BLL.Services;

public interface IExpenseService : IEntityRepository<App.BLL.DTO.Expense>, IExpenseRepositoryCustom<App.BLL.DTO.Expense>
{

    Task<App.BLL.DTO.Expense> CreateExpenseWithAttributesAsync(App.BLL.DTO.AddExpense expenseData);
    Task<double> CalculateExpensesTotal(Guid tripId);

}