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
    }

}