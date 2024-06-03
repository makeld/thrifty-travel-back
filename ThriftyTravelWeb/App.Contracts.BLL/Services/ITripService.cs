using App.BLL.DTO;
using App.Contracts.DAL.Repositories;
using Base.Contracts.DAL;

namespace App.Contracts.BLL.Services;

public interface ITripService : IEntityRepository<Trip>
{
    Task<Trip> CreateTripWithData(AddTrip tripData, Guid appUserId);
    Task UpdateTripWithData(AddTrip tripData, Guid appUserId);
    Task<AddTrip> GetTripDataById(Guid tripId);

}