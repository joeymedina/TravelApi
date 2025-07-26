using Travel.Model;

namespace Travel.Application.Interfaces;

public interface ITripsRepository
{
    public Task<Trip?> GetTrip(string id);
    public List<Trip> GetTrips();
    public Task DeleteTripAsync(string id);
    public Task UpdateTrip(Trip trip);
    public Task CreateTrip(Trip trip);
}