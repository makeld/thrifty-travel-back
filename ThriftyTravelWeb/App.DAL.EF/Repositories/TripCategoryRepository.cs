using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class TripCategoryRepository : BaseEntityRepository<AppDomain.TripCategory, DALDTO.TripCategory, AppDbContext>, ITripCategoryRepository
{
    private TripRepository _tripRepository;

    public TripCategoryRepository(AppDbContext dbContext, IMapper mapper, TripRepository tripRepository) :
        base(dbContext, new DalDomainMapper<AppDomain.TripCategory, DALDTO.TripCategory>(mapper))
    {
        _tripRepository = tripRepository;
    }
    
    public async Task<IEnumerable<DALDTO.TripCategory?>> GetAllTripCategoriesByCategoryId(Guid categoryId)
    {
        var query = CreateQuery(categoryId)
            .Where(v => v.CategoryId == categoryId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }


    public async Task<IEnumerable<DALDTO.TripCategory?>> GetAllTripCategoriesByTripId(Guid tripId)
    {
        var query = CreateQuery(tripId)
            .Where(v => v.TripId == tripId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }
}