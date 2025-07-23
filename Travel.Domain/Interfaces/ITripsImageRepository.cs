using Travel.Model;

namespace Travel.Domain.Interfaces;

public interface ITripsImageRepository
{
    public Task<List<TripImage>?> GetTripImages(string id);
    public Task<TripImage?> GetTripImage(string tripId, string id);
    public Task CreateTripImage(TripImage tripImage);
    public Task DeleteTripImageAsync(string id);
    public Task DeleteTripImageAsync(Guid id);
    public Task UpdateTripImage(TripImage trip);

}