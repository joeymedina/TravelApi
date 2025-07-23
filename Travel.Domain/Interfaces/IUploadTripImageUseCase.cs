using Travel.Model;

namespace Travel.Domain.Interfaces;

public interface IUploadTripImageUseCase
{
    public Task<(string Location, TripImage TripImage)> UploadTripImageAsync(string tripId, TripImageUploadDto fileDto);
}