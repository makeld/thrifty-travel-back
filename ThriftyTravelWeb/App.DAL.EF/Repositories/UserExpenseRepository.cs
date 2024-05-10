using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class UserExpenseRepository : BaseEntityRepository<AppDomain.UserExpense, DALDTO.UserExpense, AppDbContext>, IUserExpenseRepository
{
    public UserExpenseRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.UserExpense, DALDTO.UserExpense>(mapper))
    {
    }
}