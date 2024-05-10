using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.DAL.EF;
using AppDomain = Domain.Entities;
using DALDTO = App.DAL.DTO;

namespace App.DAL.EF.Repositories;

public class CommentRepository : BaseEntityRepository<AppDomain.Comment, DALDTO.Comment, AppDbContext>, ICommentRepository
{
    public CommentRepository(AppDbContext dbContext, IMapper mapper) : 
        base(dbContext, new DalDomainMapper<AppDomain.Comment, DALDTO.Comment>(mapper))
    {
    }
}