using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class PhotoRepository : BaseEntityRepository<AppDomain.Photo, DALDTO.Photo, AppDbContext>, IPhotoRepository
{
    public PhotoRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Photo, DALDTO.Photo>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.Photo?>> GetAllPhotosByExpenseId(Guid expenseId)
    {
        var query = CreateQuery(expenseId)
            .Where(v => v.ExpenseId == expenseId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }

    public async Task<IEnumerable<DALDTO.Photo?>> GetAllPhotosByTripId(Guid tripId)
    {
        var query = CreateQuery(tripId)
            .Where(v => v.TripId == tripId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }
}