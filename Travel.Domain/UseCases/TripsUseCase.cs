using Travel.Domain.Interfaces;
using Travel.Model;

namespace Travel.Domain.UseCases;

public class TripsUseCase(ITripsRepository tripsRepository) : ITripsUseCase
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
        await tripsRepository.CreateTrip(trip);
    }
    public void DeleteTrip(string id)
    {
        tripsRepository.DeleteTrip(id);
    }

    public async Task UpdateTrip(Trip trip)
    {
        await tripsRepository.UpdateTrip(trip);
    }
}