using AutoMapper;
using Travel.Application.Interfaces;
using Travel.Domain.Entities;
using Travel.Model;
using Travel.Model.TripImage;

namespace Travel.Application.UseCases;

public class UploadTripImageUseCase(
    IMinioService minioService,
    ITripsImageService tripsImageService,
    IMapper mapper) : IUploadTripImageUseCase
{
    private readonly IMinioService _minioService = minioService;
    private readonly ITripsImageService _tripsImageService = tripsImageService;
    private readonly IMapper _mapper = mapper;

    public async Task<(string Location, TripImageEntity TripImage)> UploadTripImageAsync(string tripId, TripImageUpload file)
    {
        var tripIdLower = tripId.ToLowerInvariant();
        //TODO: add retries if minio succeeds but database fails
        var objectName = file.FileName;

        var exists = await _minioService.BucketExistsAsync(tripIdLower);
        if (!exists)
        {
            await _minioService.MakeBucketAsync(tripIdLower);
            await _minioService.SetBucketPolicyToPublicAsync(tripIdLower);
        }

        using var stream = new MemoryStream(file.Content);
        await _minioService.PutObjectStreamAsync(tripIdLower, objectName, stream, file);

        var tripImage = new TripImageEntity
        {
            TripId = Guid.Parse(tripIdLower),
            Url = $"http://localhost:9000/{tripIdLower}/{file.FileName}",
            Caption = ""
        };

        await _tripsImageService.CreateTripImage(tripImage);

        var location = $"api/trips/{tripIdLower}/images/{tripImage.Id}";
        return (location, tripImage);
    }
}
