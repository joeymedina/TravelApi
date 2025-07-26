using Travel.Application.Interfaces;
using Travel.Model;

namespace Travel.Application.Services;

public class TripsService(ITripsRepository tripsRepository) : ITripsService
{
    public async Task<Trip?> GetTrip(string id)
    {
        return await tripsRepository.GetTrip(id);
    }

    public List<Trip> GetTrips()
    {
        return tripsRepository.GetTrips();
    }

    public async Task CreateTrip(Trip trip)
    {
        //TODO: change startdate and enddate to DateTimeOffset type
        trip.StartDate = DateTime.SpecifyKind(trip.StartDate, DateTimeKind.Utc);
        trip.EndDate = DateTime.SpecifyKind(trip.EndDate, DateTimeKind.Utc);
        await tripsRepository.CreateTrip(trip);
    }
    public async Task DeleteTripAsync(string id)
    {
        await tripsRepository.DeleteTripAsync(id);
    }

    public async Task UpdateTrip(Trip trip)
    {
        //TODO: change startdate and enddate to DateTimeOffset type
        trip.StartDate = DateTime.SpecifyKind(trip.StartDate, DateTimeKind.Utc);
        trip.EndDate = DateTime.SpecifyKind(trip.EndDate, DateTimeKind.Utc);
        await tripsRepository.UpdateTrip(trip);
    }
}