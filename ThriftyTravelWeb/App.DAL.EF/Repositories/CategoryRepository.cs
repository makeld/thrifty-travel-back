using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Domain.Entities;

namespace App.DAL.EF.Repositories;

public class ContestRepository : BaseEntityRepository<Category, Category, AppDbContext>,  ICategoryRepository
{
    public ContestRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<Category, Category>(mapper))
    {
    }
}