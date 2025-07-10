using Travel.Model;

namespace Travel.Domain.Interfaces;

public interface ITripsImageService
{
    public Task<List<TripImage>?> GetTripImages(string id);
    public Task<TripImage?> GetTripImage(string tripId, string id);
    public Task CreateTripImage(TripImage tripImage);
    public void DeleteTripImage(string id);
    public Task DeleteTripImages(string id);
    public Task UpdateTripImage(TripImage trip);
}