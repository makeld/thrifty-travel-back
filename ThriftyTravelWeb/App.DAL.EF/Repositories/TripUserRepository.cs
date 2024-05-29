using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class TripUserRepository : BaseEntityRepository<AppDomain.TripUser, DALDTO.TripUser, AppDbContext>, ITripUserRepository
{
    public TripUserRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.TripUser, DALDTO.TripUser>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.TripUser?>> GetAllTripUsersByAppUserId(Guid appUserId)
    {
        var query = CreateQuery(appUserId)
            .Where(v => v.AppUserId == appUserId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }

    public async Task<IEnumerable<DALDTO.TripUser?>> GetAllTripUsersByTripId(Guid tripId)
    {
        var query = CreateQuery(tripId)
            .Where(v => v.TripId == tripId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }
}