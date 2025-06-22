using Travel.Domain.Interfaces;
using Travel.Model;

namespace Travel.Domain.UseCases;

public class TripsImageUseCase(ITripsImageRepository tripsImageRepository) : ITripsImageUseCase
{
    ITripsImageRepository _tripsImageRepository = tripsImageRepository;

    public async Task<List<TripImage>?> GetTripImages(string id)
    {
       return await _tripsImageRepository.GetTripImages(id);
    }

    public async Task<TripImage?> GetTripImage(string tripId, string id)
    {
        return await _tripsImageRepository.GetTripImage(tripId, id);
    }

    public async Task CreateTripImage(TripImage tripImage)
    {
        await _tripsImageRepository.CreateTripImage(tripImage);
    }
    
    public void DeleteTripImage(string id)
    {
        _tripsImageRepository.DeleteTripImage(id);
    }
    
    public async Task UpdateTripImage(TripImage tripImage)
    {
        await _tripsImageRepository.UpdateTripImage(tripImage);
    }
}