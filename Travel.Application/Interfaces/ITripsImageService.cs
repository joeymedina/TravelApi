using Travel.Domain.Entities;

namespace Travel.Application.Interfaces;

public interface ITripsImageService
{
    public Task<List<TripImageEntity>?> GetTripImages(string id);
    public Task<TripImageEntity?> GetTripImage(string tripId, string id);
    public Task CreateTripImage(TripImageEntity tripImage);
    public Task DeleteTripImageAsync(string id);
    public Task DeleteTripImagesAsync(string id);
    public Task UpdateTripImage(TripImageEntity trip);
}