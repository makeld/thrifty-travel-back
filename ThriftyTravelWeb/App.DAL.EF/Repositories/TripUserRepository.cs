using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class TripUserRepository : BaseEntityRepository<AppDomain.TripUser, DALDTO.TripUser, AppDbContext>, ITripUserRepository
{
    public TripUserRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.TripUser, DALDTO.TripUser>(mapper))
    {
    }
}