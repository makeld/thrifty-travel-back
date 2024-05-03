using Base.Contracts.DAL;
using Domain.Entities;

namespace App.Contracts.DAL.Repositories;

public interface ICategoryRepository : IEntityRepository<Category>
{
    // define your DAL only custom methods here
}