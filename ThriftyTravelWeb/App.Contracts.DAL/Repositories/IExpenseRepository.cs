using Base.Contracts.DAL;
using Domain.Entities;

namespace App.Contracts.DAL.Repositories;

public interface IExpenseRepository : IEntityRepository<Expense>
{
    // define your DAL only custom methods here
}