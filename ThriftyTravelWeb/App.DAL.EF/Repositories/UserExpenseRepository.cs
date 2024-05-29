using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class UserExpenseRepository : BaseEntityRepository<AppDomain.UserExpense, DALDTO.UserExpense, AppDbContext>, IUserExpenseRepository
{
    public UserExpenseRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.UserExpense, DALDTO.UserExpense>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.UserExpense?>> GetAllUserExpensesByTripUserId(Guid tripUserId)
    {
        var query = CreateQuery(tripUserId)
            .Where(v => v.TripUserId == tripUserId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }

    public async Task<IEnumerable<DALDTO.UserExpense?>> GetAllUserExpensesByExpenseId(Guid expenseId)
    {
        var query = CreateQuery(expenseId)
            .Where(v => v.ExpenseId == expenseId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }
}