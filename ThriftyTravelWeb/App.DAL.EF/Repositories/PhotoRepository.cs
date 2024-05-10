using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class PhotoRepository : BaseEntityRepository<AppDomain.Photo, DALDTO.Photo, AppDbContext>, IPhotoRepository
{
    public PhotoRepository(AppDbContext dbContext, IMapper mapper) :
        base(dbContext, new DalDomainMapper<AppDomain.Photo, DALDTO.Photo>(mapper))
    {
    } 
}