using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class TripLocationRepository : BaseEntityRepository<AppDomain.TripLocation, DALDTO.TripLocation, AppDbContext>, ITripLocationRepository
{
    public TripLocationRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.TripLocation, DALDTO.TripLocation>(mapper))
    {
    }
}