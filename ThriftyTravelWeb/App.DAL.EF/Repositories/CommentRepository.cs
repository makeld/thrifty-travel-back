using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using Microsoft.EntityFrameworkCore;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class CommentRepository : BaseEntityRepository<AppDomain.Comment, DALDTO.Comment, AppDbContext>, ICommentRepository
{
    public CommentRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Comment, DALDTO.Comment>(mapper))
    {
    }

    public async Task<IEnumerable<DALDTO.Comment?>> GetAllCommentsByAppUserId(Guid appUserId)
    {
        var query = CreateQuery(appUserId)
            .Where(v => v.AppUserId == appUserId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }

    public async Task<IEnumerable<DALDTO.Comment?>> GetAllCommentsByTripId(Guid tripId)
    {
        var query = CreateQuery(tripId)
            .Where(v => v.TripId == tripId);
        var result = await query.ToListAsync();
        return result.Select(v => Mapper.Map(v));
    }
}