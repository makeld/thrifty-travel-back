using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class CategoryRepository : BaseEntityRepository<AppDomain.Category, DALDTO.Category, AppDbContext>, ICategoryRepository
{
    public CategoryRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Category, DALDTO.Category>(mapper))
    {
    }
}