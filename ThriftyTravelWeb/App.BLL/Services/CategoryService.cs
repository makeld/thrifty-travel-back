using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services;

public class CategoryService :
    BaseEntityService<App.DAL.DTO.Category, App.BLL.DTO.Category, ICategoryRepository>,
    ICategoryService
{
    public CategoryService(IAppUnitOfWork uoW, ICategoryRepository repository, IMapper mapper) : base(uoW,
        repository, new BllDalMapper<App.DAL.DTO.Category, App.BLL.DTO.Category>(mapper))
    {
    }
}