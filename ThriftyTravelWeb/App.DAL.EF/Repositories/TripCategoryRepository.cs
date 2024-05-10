using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class TripCategoryRepository : BaseEntityRepository<AppDomain.TripCategory, DALDTO.TripCategory, AppDbContext>, ITripCategoryRepository
{
    public TripCategoryRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.TripCategory, DALDTO.TripCategory>(mapper))
    {
    }
    
}