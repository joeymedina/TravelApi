using Travel.Model;

namespace Travel.Application.Interfaces;

public interface IUploadTripImageUseCase
{
    public Task<(string Location, TripImage TripImage)> UploadTripImageAsync(string tripId, TripImageUploadDto fileDto);
}