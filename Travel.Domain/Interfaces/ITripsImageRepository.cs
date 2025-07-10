using Travel.Model;

namespace Travel.Domain.Interfaces;

public interface ITripsImageRepository
{
    public Task<List<TripImage>?> GetTripImages(string id);
    public Task<TripImage?> GetTripImage(string tripId, string id);
    public Task CreateTripImage(TripImage tripImage);
    public void DeleteTripImage(string id);
    public void DeleteTripImage(Guid id);
    public Task UpdateTripImage(TripImage trip);

}