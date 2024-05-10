using App.DAL.DTO;
using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface IExpenseRepository : IEntityRepository<App.DAL.DTO.Expense>
{
    // define your DAL only custom methods here
}