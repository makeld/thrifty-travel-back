using App.Contracts.BLL.Services;
using Base.Contracts.BLL;
using Domain.Identity;

namespace App.Contracts.BLL;

public interface IAppBLL : IBLL
{
    ITripService TripService { get; }

    ICategoryService CategoryService { get; }

    IExpenseService ExpenseService { get; }

    IEntityService<AppUser> AppUserService { get; }

    ICommentService CommentService { get; }

    ICountryService CountryService { get; }

    ILikeService LikeService { get; }

    ILocationService LocationService { get; }

    IPhotoService PhotoService { get; }

    ITripCategoryService TripCategoryService { get; }

    ITripLocationService TripLocationService { get; }

    ITripUserService TripUserService { get; }

    IUserExpenseService UserExpenseService { get; }
}