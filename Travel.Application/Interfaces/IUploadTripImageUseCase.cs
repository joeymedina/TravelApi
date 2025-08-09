using Travel.Domain.Entities;
using Travel.Model.TripImage;

namespace Travel.Application.Interfaces;

public interface IUploadTripImageUseCase
{
    public Task<(string Location, TripImageEntity TripImage)> UploadTripImageAsync(string tripId, TripImageUpload file);
}