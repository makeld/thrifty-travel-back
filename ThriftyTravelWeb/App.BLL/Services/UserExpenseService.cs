using App.BLL.DTO;
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

    public async Task<IEnumerable<UserExpense?>> GetAllUserExpensesByTripUserId(Guid tripUserId)
    {
        return (await Repository.GetAllUserExpensesByTripUserId(tripUserId)).Select(e => Mapper.Map(e));

    }

    public async Task<IEnumerable<UserExpense?>> GetAllUserExpensesByExpenseId(Guid expenseId)
    {
        return (await Repository.GetAllUserExpensesByExpenseId(expenseId)).Select(e => Mapper.Map(e));
    }
}