using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;
using Domain.Entities;
using Domain.Identity;
using App.DAL.DTO;

namespace App.Contracts.DAL;

public interface IAppUnitOfWork : IUnitOfWork
{
    ICategoryRepository CategoryRepository { get; }
    
    IExpenseRepository ExpenseRepository { get; }
    
    ITripRepository TripRepository { get; }
    IEntityRepository<AppUser> AppUserRepository { get; }
    ICommentRepository CommentRepository { get; }
    ICountryRepository CountryRepository { get; }
    ILikeRepository LikeRepository { get; }
    ILocationRepository LocationRepository { get; }
    IPhotoRepository PhotoRepository { get; }
    ITripCategoryRepository TripCategoryRepository { get; }
    ITripUserRepository TripUserRepository { get; }
    ITripLocationRepository TripLocationRepository { get; }
    IUserExpenseRepository UserExpenseRepository { get; }
}