using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using App.DAL.EF.Repositories;
using AutoMapper;
using Base.Contracts.DAL;
using Base.DAL.EF;
using Domain.Identity;
using App.DAL.DTO;

namespace App.DAL.EF;

public class AppUOW : BaseUnitOfWork<AppDbContext>, IAppUnitOfWork
{
    private readonly IMapper _mapper;
    public AppUOW(AppDbContext dbContext, IMapper mapper) : base(dbContext)
    {
        _mapper = mapper;
    }

    private ICategoryRepository? _categoryRepository;
    public ICategoryRepository CategoryRepository => _categoryRepository ?? new CategoryRepository(UowDbContext, _mapper);
    
    private ITripRepository? _tripRepository;
    public ITripRepository TripRepository => _tripRepository ?? new TripRepository(UowDbContext, _mapper);

    private IExpenseRepository? _expenseRepository;
    public IExpenseRepository ExpenseRepository => _expenseRepository ?? new ExpenseRepository(UowDbContext, _mapper);

    private IEntityRepository<AppUser>? _appUserRepository;
    public IEntityRepository<AppUser> AppUserRepository => _appUserRepository ?? new BaseEntityRepository<AppUser, AppUser, AppDbContext>(UowDbContext, 
        new DalDomainMapper<AppUser, AppUser>(_mapper));

    private ICommentRepository? _commentRepository;
    public ICommentRepository CommentRepository => _commentRepository ?? new CommentRepository(UowDbContext, _mapper);

    private ICountryRepository? _countryRepository;
    public ICountryRepository CountryRepository => _countryRepository ?? new CountryRepository(UowDbContext, _mapper);

    private ILikeRepository? _likeRepository;
    public ILikeRepository LikeRepository => _likeRepository ?? new LikeRepository(UowDbContext, _mapper);

    private ILocationRepository? _locationRepository;
    public ILocationRepository LocationRepository => _locationRepository ?? new LocationRepository(UowDbContext, _mapper);
    
    private IPhotoRepository? _photo;
    public IPhotoRepository PhotoRepository => _photo ?? new PhotoRepository(UowDbContext, _mapper);

    private ITripCategoryRepository? _tripCategory;
    public ITripCategoryRepository TripCategoryRepository => _tripCategory ?? new TripCategoryRepository(UowDbContext, _mapper, new TripRepository(UowDbContext, _mapper));

    private ITripLocationRepository? _tripLocation;
    public ITripLocationRepository TripLocationRepository => _tripLocation ?? new TripLocationRepository(UowDbContext, _mapper);

    private ITripUserRepository? _tripUser;
    public ITripUserRepository TripUserRepository => _tripUser ?? new TripUserRepository(UowDbContext, _mapper);

    private IUserExpenseRepository? _userExpense;
    public IUserExpenseRepository UserExpenseRepository => _userExpense ?? new UserExpenseRepository(UowDbContext, _mapper);
}