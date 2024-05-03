using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;
using Domain.Entities;
using Domain.Identity;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    
    IExpenseRepository ExpenseRepository { get; }
    
    ITripRepository TripRepository { get; }
    IEntityRepository<AppUser> AppUserRepository { get; }
}