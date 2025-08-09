using Travel.Domain.Entities;

namespace Travel.Application.Interfaces;

public interface ITripsService
{
    public Task<TripEntity?> GetTrip(string id);
    public List<TripEntity> GetTrips();

    public Task DeleteTripAsync(string id);
    public Task UpdateTrip(TripEntity trip);
    public Task CreateTrip(TripEntity trip);

}