using App.Contracts.DAL.Repositories;
using AutoMapper;
using DALDTO = App.DAL.DTO;
using Base.DAL.EF;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.EF.Repositories;

public class TripRepository : BaseEntityRepository<Trip, DALDTO.Trip, AppDbContext>,
    ITripRepository
{
    public TripRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<Trip, DALDTO.Trip>(mapper))
    {
    }

    // implement your custom methods here
    public async Task<IEnumerable<DALDTO.Trip>> GetAllSortedAsync(Guid userId)
    {
        var query = CreateQuery(userId);
        var res = await query.ToListAsync();
        
        return res.Select(e => Mapper.Map(e));
    }
}