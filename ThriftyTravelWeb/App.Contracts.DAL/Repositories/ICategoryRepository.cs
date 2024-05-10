using Base.Contracts.DAL;

namespace App.Contracts.DAL.Repositories;

public interface ICategoryRepository : IEntityRepository<App.DAL.DTO.Category>
{
    // define your DAL only custom methods here
}