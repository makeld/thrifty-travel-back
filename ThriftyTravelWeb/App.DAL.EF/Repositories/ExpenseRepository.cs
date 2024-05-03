using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Domain.Entities;

namespace App.DAL.EF.Repositories;

public class ExpenseRepository : BaseEntityRepository<Expense, Expense, AppDbContext>,  IExpenseRepository
{
    public ExpenseRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<Expense, Expense>(mapper))
    {
    }
}