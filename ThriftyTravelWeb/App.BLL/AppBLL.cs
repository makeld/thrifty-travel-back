using App.BLL.Services;
using App.Contracts.BLL;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.DAL.EF;
using AutoMapper;
using Base.BLL;
using Base.Contracts.BLL;
using Base.Contracts.DAL;
using Domain.Identity;

namespace App.BLL;

public class AppBLL: BaseBLL<AppDbContext>, IAppBLL
{
    private readonly IMapper _mapper;
    private readonly IAppUnitOfWork _uow;
    
    public AppBLL(IAppUnitOfWork uoW, IMapper mapper) : base(uoW)
    {
        _mapper = mapper;
        _uow = uoW;
    }

    private ITripService? _trips;
    public ITripService TripService => _trips ?? new TripService(_uow, _uow.TripRepository, _uow.TripUserRepository, _mapper);

    private ICategoryService? _categories;
    public ICategoryService CategoryService => _categories ?? new CategoryService(_uow, _uow.CategoryRepository, _mapper);
    
    private IExpenseService? _expenses;
    public IExpenseService ExpenseService => _expenses ?? new ExpenseService(_uow, _uow.ExpenseRepository, _uow.CountryRepository, _uow.LocationRepository, _uow.TripLocationRepository, _mapper);

    private IEntityService<AppUser>? _users;
    public IEntityService<AppUser> AppUserService => _users ??
                                               new BaseEntityService<AppUser, AppUser, IEntityRepository<AppUser>>(UoW, _uow.AppUserRepository,
                                                   new BllDalMapper<AppUser, AppUser>(_mapper));

    private ICommentService? _comment;
    public ICommentService CommentService => _comment ?? new CommentService(_uow, _uow.CommentRepository, _mapper);
    
    private ICountryService? _country;
    public ICountryService CountryService => _country ?? new CountryService(_uow, _uow.CountryRepository, _mapper);
    
    private ILikeService? _like;
    public ILikeService LikeService => _like ?? new LikeService(_uow, _uow.LikeRepository, _mapper);
    
    private ILocationService? _location;
    public ILocationService LocationService => _location ?? new LocationService(_uow, _uow.LocationRepository, _mapper);
    
    private IPhotoService? _photo;
    public IPhotoService PhotoService => _photo ?? new PhotoService(_uow, _uow.PhotoRepository, _mapper);
    
    private ITripCategoryService? _tripCategory;
    public ITripCategoryService TripCategoryService => _tripCategory ?? new TripCategoryService(_uow, _uow.TripCategoryRepository, _mapper);
    
    private ITripLocationService? _tripLocation;
    public ITripLocationService TripLocationService => _tripLocation ?? new TripLocationService(_uow, _uow.TripLocationRepository, _mapper);
    
    private ITripUserService? _tripUser;
    public ITripUserService TripUserService => _tripUser ?? new TripUserService(_uow, _uow.TripUserRepository, _mapper);
    
    private IUserExpenseService? _userExpense;
    public IUserExpenseService UserExpenseService => _userExpense ?? new UserExpenseService(_uow, _uow.UserExpenseRepository, _mapper);
}