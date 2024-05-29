using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class ExpenseRepository : BaseEntityRepository<AppDomain.Expense, DALDTO.Expense, AppDbContext>, IExpenseRepository
{
    public ExpenseRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Expense, DALDTO.Expense>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.Expense?>> GetExpenseByTripId(Guid tripId)
    {
        var query = CreateQuery(tripId)
            .Where(v => v.TripId == tripId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }
}