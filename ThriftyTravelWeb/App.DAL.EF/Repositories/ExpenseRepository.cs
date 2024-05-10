using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class ExpenseRepository : BaseEntityRepository<AppDomain.Expense, DALDTO.Expense, AppDbContext>, IExpenseRepository
{
    public ExpenseRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Expense, DALDTO.Expense>(mapper))
    {
    }
}