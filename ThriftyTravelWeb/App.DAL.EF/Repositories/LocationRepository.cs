using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class LocationRepository : BaseEntityRepository<AppDomain.Location, DALDTO.Location, AppDbContext>, ILocationRepository
{
    public LocationRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.Location, DALDTO.Location>(mapper))
    {
    }
}