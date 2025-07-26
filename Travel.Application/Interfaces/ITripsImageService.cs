using Travel.Model;

namespace Travel.Application.Interfaces;

public interface ITripsImageService
{
    public Task<List<TripImage>?> GetTripImages(string id);
    public Task<TripImage?> GetTripImage(string tripId, string id);
    public Task CreateTripImage(TripImage tripImage);
    public Task DeleteTripImageAsync(string id);
    public Task DeleteTripImagesAsync(string id);
    public Task UpdateTripImage(TripImage trip);
}