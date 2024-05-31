using App.BLL.DTO;
using App.Contracts.BLL.Services;
using App.Contracts.DAL;
using App.Contracts.DAL.Repositories;
using AutoMapper;
using Base.BLL;

namespace App.BLL.Services
{
    public class TripService : BaseEntityService<App.DAL.DTO.Trip, App.BLL.DTO.Trip, ITripRepository>, ITripService
    {
        private readonly ITripUserRepository _tripUserRepository;
        private readonly IMapper _mapper;
        private readonly IAppUnitOfWork _uow;

        public TripService(IAppUnitOfWork uoW, ITripRepository repository, ITripUserRepository tripUserRepository, IMapper mapper) 
            : base(uoW, repository, new BllDalMapper<App.DAL.DTO.Trip, App.BLL.DTO.Trip>(mapper))
        {
            _tripUserRepository = tripUserRepository;
            _uow = uoW;
            _mapper = mapper;
        }

        public async Task<Trip> CreateTripWithUserAsync(Trip trip, Guid appUserId)
        {
            var createdTrip = Add(trip);
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

            return createdTrip;
        }
    }

}