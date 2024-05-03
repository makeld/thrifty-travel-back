using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Domain.Entities;
using Domain.Identity;

namespace App.DAL.EF;

public class AppUOW : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    private ICategoryRepository? _categoryRepository;
    public ICategoryRepository CategoryRepository => _categoryRepository ?? new ContestRepository(UowDbContext, _mapper);
    
    private ITripRepository? _tripRepository;
    public ITripRepository TripRepository => _tripRepository ?? new TripRepository(UowDbContext, _mapper);

    private IExpenseRepository? _expenseRepository;
    public IExpenseRepository ExpenseRepository => _expenseRepository ?? new ExpenseRepository(UowDbContext, _mapper);

    private IEntityRepository<AppUser>? _appUserRepository;
    public IEntityRepository<AppUser> AppUserRepository => _appUserRepository ?? new BaseEntityRepository<AppUser, AppUser, AppDbContext>(UowDbContext, 
        new DalDomainMapper<AppUser, AppUser>(_mapper));

}