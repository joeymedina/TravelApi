using Travel.Application.Interfaces;
using Travel.Domain.Entities;
using Travel.Model;

namespace Travel.Application.Services;

public class TripsImageService(ITripsImageRepository tripsImageRepository) : ITripsImageService
{
    ITripsImageRepository _tripsImageRepository = tripsImageRepository;

    public async Task<List<TripImageEntity>?> GetTripImages(string id)
    {
       return await _tripsImageRepository.GetTripImages(id);
    }

    public async Task<TripImageEntity?> GetTripImage(string tripId, string id)
    {
        return await _tripsImageRepository.GetTripImage(tripId, id);
    }

    public async Task CreateTripImage(TripImageEntity tripImage)
    {
        await _tripsImageRepository.CreateTripImage(tripImage);
    }
    
    public async Task DeleteTripImageAsync(string id)
    {
        await _tripsImageRepository.DeleteTripImageAsync(id);
    }

    public async Task DeleteTripImagesAsync(string id)
    {
        var images = await GetTripImages(id);
        if (images != null)
        {
            foreach (var image in images)
            {
                await _tripsImageRepository.DeleteTripImageAsync(image.Id);
            }
        }
    }
    public async Task UpdateTripImage(TripImageEntity tripImage)
    {
        await _tripsImageRepository.UpdateTripImage(tripImage);
    }
}