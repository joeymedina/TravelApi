using Travel.Model;

namespace Travel.Domain.Interfaces;

public interface ITripsService
{
    public Task<Trip?> GetTrip(string id);
    public List<Trip> GetTrips();

    public void DeleteTrip(string id);
    public Task UpdateTrip(Trip trip);
    public Task CreateTrip(Trip trip);

}