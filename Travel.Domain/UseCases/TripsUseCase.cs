using Travel.Domain.Interfaces;
using Travel.Model;

namespace Travel.Domain.UseCases;

public class TripsUseCase(ITravelRepository travelRepository) : ITripsUseCase
{
    public async Task<Trip?> GetTrip(string id)
    {
        return await travelRepository.GetTrip(id);
    }

    public List<Trip> GetTrips()
    {
        return travelRepository.GetTrips();
    }

    public async Task CreateTrip(Trip trip)
    {
        await travelRepository.CreateTrip(trip);
    }
    public void DeleteTrip(string id)
    {
        travelRepository.DeleteTrip(id);
    }

    public async Task UpdateTrip(Trip trip)
    {
        await travelRepository.UpdateTrip(trip);
    }
}