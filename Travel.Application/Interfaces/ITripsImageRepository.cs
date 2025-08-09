using Travel.Domain.Entities;

namespace Travel.Application.Interfaces;

public interface ITripsImageRepository
{
    public Task<List<TripImageEntity>?> GetTripImages(string id);
    public Task<TripImageEntity?> GetTripImage(string tripId, string id);
    public Task CreateTripImage(TripImageEntity tripImage);
    public Task DeleteTripImageAsync(string id);
    public Task DeleteTripImageAsync(Guid id);
    public Task UpdateTripImage(TripImageEntity trip);

}