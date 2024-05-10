using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class CommentService :
    BaseEntityService<App.DAL.DTO.Comment, App.BLL.DTO.Comment, ICommentRepository>,
    ICommentService
{
    public CommentService(IAppUnitOfWork uoW, ICommentRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Comment, App.BLL.DTO.Comment>(mapper))
    {
    }
}