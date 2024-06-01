using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class TripLocationRepository : BaseEntityRepository<AppDomain.TripLocation, DALDTO.TripLocation, AppDbContext>, ITripLocationRepository
{
    public TripLocationRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.TripLocation, DALDTO.TripLocation>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.TripLocation?>> GetAllTripLocationsByLocationId(Guid locationId)
    {
        var query = CreateQuery(locationId)
            .Where(v => v.LocationId == locationId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }

    public async Task<IEnumerable<DALDTO.TripLocation?>> GetAllTripLocationsByTripId(Guid tripId)
    {
        var query = CreateQuery(tripId)
            .Where(v => v.TripId == tripId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }
}