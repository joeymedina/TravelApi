using Travel.Domain.Interfaces;
using Travel.Model;

namespace Travel.Domain.Services;

public class TripsImageService(ITripsImageRepository tripsImageRepository) : ITripsImageService
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

    public async Task DeleteTripImages(string id)
    {
        var images = await GetTripImages(id);
        if (images != null)
        {
            foreach (var image in images)
            {
                _tripsImageRepository.DeleteTripImage(image.Id);
            }
        }
    }
    public async Task UpdateTripImage(TripImage tripImage)
    {
        await _tripsImageRepository.UpdateTripImage(tripImage);
    }
}