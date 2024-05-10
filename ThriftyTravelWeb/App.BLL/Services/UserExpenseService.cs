using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;

namespace App.BLL.Services;

public class UserExpenseService :
    BaseEntityService<App.DAL.DTO.UserExpense, App.BLL.DTO.UserExpense, IUserExpenseRepository>,
    IUserExpenseService
{
    public UserExpenseService(IAppUnitOfWork uoW, IUserExpenseRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.UserExpense, App.BLL.DTO.UserExpense>(mapper))
    {
    }
}