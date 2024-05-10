using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class LikeRepository : BaseEntityRepository<AppDomain.Like, DALDTO.Like, AppDbContext>, ILikeRepository
{
    public LikeRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.Like, DALDTO.Like>(mapper))
    {
    }
}