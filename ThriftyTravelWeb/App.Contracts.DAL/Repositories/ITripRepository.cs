using Base.Contracts.DAL;
using Domain.Entities;

namespace App.Contracts.DAL.Repositories;

public interface ITripRepository : IEntityRepository<App.DAL.DTO.Trip>
{
    // define your DAL only custom methods here
}
