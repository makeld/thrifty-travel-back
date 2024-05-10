using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class ExpenseService :
    BaseEntityService<App.DAL.DTO.Expense, App.BLL.DTO.Expense, IExpenseRepository>,
    IExpenseService
{
    public ExpenseService(IAppUnitOfWork uoW, IExpenseRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Expense, App.BLL.DTO.Expense>(mapper))
    {
    }
}