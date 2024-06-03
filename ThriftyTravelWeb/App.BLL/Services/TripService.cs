using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;
using Microsoft.IdentityModel.Tokens;

namespace App.BLL.Services
{
    public class TripService : BaseEntityService<App.DAL.DTO.Trip, App.BLL.DTO.Trip, ITripRepository>, ITripService
    {
        private readonly ITripUserRepository _tripUserRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITripCategoryRepository _tripCategoryRepository;
        private readonly IMapper _mapper;
        private readonly IAppUnitOfWork _uow;

        public TripService(IAppUnitOfWork uoW, ITripRepository repository, ITripUserRepository tripUserRepository, IPhotoRepository photoRepository, 
            ICategoryRepository categoryRepository, ITripCategoryRepository tripCategoryRepository, IMapper mapper) 
            : base(uoW, repository, new BllDalMapper<App.DAL.DTO.Trip, App.BLL.DTO.Trip>(mapper))
        {
            _tripUserRepository = tripUserRepository;
            _photoRepository = photoRepository;
            _categoryRepository = categoryRepository;
            _tripCategoryRepository = tripCategoryRepository;
            _uow = uoW;
            _mapper = mapper;
        }

        public async Task<Trip> CreateTripWithData(AddTrip tripData, Guid appUserId)
        {
            var newTrip = new Trip()
            {
                Id = Guid.NewGuid(),
                Title = tripData.TripTitle,
                Description = tripData.TripDescription,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            
            var createdTrip = Add(newTrip);
            await _uow.SaveChangesAsync();

            var tripUser = new TripUser
            {
                Id = Guid.NewGuid(),
                TripId = createdTrip.Id,
                AppUserId = appUserId
            };
            
            var dalTripUser = _mapper.Map<App.DAL.DTO.TripUser>(tripUser);

            _tripUserRepository.Add(dalTripUser);
            await _uow.SaveChangesAsync();

            var category = new Category()
            {
                Id = Guid.NewGuid(),
                Name = tripData.TripCategory!
            };

            var dalCategory = _mapper.Map<App.DAL.DTO.Category>(category);
            _categoryRepository.Add(dalCategory);
            await _uow.SaveChangesAsync();
            
            var tripCategory = new TripCategory()
            {
                Id = Guid.NewGuid(),
                TripId = createdTrip.Id,
                CategoryId = category.Id
            };

            var dalTripCategory = _mapper.Map<App.DAL.DTO.TripCategory>(tripCategory);
            _tripCategoryRepository.Add(dalTripCategory);
            await _uow.SaveChangesAsync();


            if (!string.IsNullOrEmpty(tripData.ImageUrl))
            {
                var photo = new Photo()
                {
                    Id = Guid.NewGuid(),
                    TripId = createdTrip.Id,
                    ImageUrl = tripData.ImageUrl!,
                    Description = tripData.ImageDescription
                };
                
                var dalPhoto = _mapper.Map<App.DAL.DTO.Photo>(photo);

                _photoRepository.Add(dalPhoto);
                await _uow.SaveChangesAsync();
            }

            return createdTrip;
        }

        public async Task<AddTrip> GetTripDataById(Guid tripId)
        {
            var trip = await Repository.FirstOrDefaultAsync(tripId);
            if (trip == null)
            {
                throw new Exception("Trip not found");
            }

            var tripUsers = await _tripUserRepository.GetAllTripUsersByTripId(tripId);
            var tripUser = tripUsers.FirstOrDefault();

            var tripCategories = await _tripCategoryRepository.GetAllTripCategoriesByTripId(tripId);
            var tripCategory = tripCategories.FirstOrDefault();

            var category = await _categoryRepository.FirstOrDefaultAsync(tripCategory!.CategoryId);

            var photos = await _photoRepository.GetAllPhotosByTripId(tripId);
            var photo = photos!.FirstOrDefault() ?? null;

            return new AddTrip
            {
                TripTitle = trip.Title,
                TripDescription = trip.Description,
                TripCategory = category?.Name,
                TripId = trip.Id,
                TripUserId = tripUser?.Id,
                CategoryId = category?.Id,
                TripCategoryId = tripCategory?.Id,
                ImageId = photo?.Id,
                ImageUrl = photo?.ImageUrl,
                ImageDescription = photo?.Description
            };
        }

        public async Task UpdateTripWithData(AddTrip tripData, Guid appUserId)
        {
            var existingTrip = await Repository.FirstOrDefaultAsync(tripData.TripId!.Value);
            if (existingTrip == null)
            {
                throw new Exception("Trip not found");
            }

            existingTrip.Title = tripData.TripTitle;
            existingTrip.Description = tripData.TripDescription;
            existingTrip.UpdatedAt = DateTime.Now;
            
            Repository.Update(existingTrip);
            await _uow.SaveChangesAsync();

            var tripUser = await _tripUserRepository.FirstOrDefaultAsync(tripData.TripUserId!.Value);
            if (tripUser != null)
            {
                tripUser.AppUserId = appUserId;
                _tripUserRepository.Update(tripUser);
                await _uow.SaveChangesAsync();
            }

            var category = await _categoryRepository.FirstOrDefaultAsync(tripData.CategoryId!.Value);
            if (category != null)
            {
                category.Name = tripData.TripCategory!;
                _categoryRepository.Update(category);
                await _uow.SaveChangesAsync();
            }

            var tripCategory = await _tripCategoryRepository.FirstOrDefaultAsync(tripData.TripCategoryId!.Value);
            if (tripCategory != null)
            {
                tripCategory.CategoryId = category!.Id;
                _tripCategoryRepository.Update(tripCategory);
                await _uow.SaveChangesAsync();
            }

            if (!string.IsNullOrEmpty(tripData.ImageUrl))
            {
                var photo = await _photoRepository.FirstOrDefaultAsync(tripData.ImageId!.Value);
                if (photo != null)
                {
                    photo.ImageUrl = tripData.ImageUrl!;
                    photo.Description = tripData.ImageDescription;
                    _photoRepository.Update(photo);
                    await _uow.SaveChangesAsync();
                }
                else
                {
                    var newPhoto = new Photo()
                    {
                        Id = Guid.NewGuid(),
                        TripId = existingTrip.Id,
                        ImageUrl = tripData.ImageUrl!,
                        Description = tripData.ImageDescription
                    };

                    var dalPhoto = _mapper.Map<App.DAL.DTO.Photo>(newPhoto);
                    _photoRepository.Add(dalPhoto);
                    await _uow.SaveChangesAsync();
                }
            }
        }
    }

}