using AutoMapper;
using Travel.Application.Interfaces;
using Travel.Model;

namespace Travel.Application.UseCases;

public class UploadTripImageUseCase(
    IMinioService minioService,
    ITripsImageService tripsImageService,
    IMapper mapper) : IUploadTripImageUseCase
{
    private readonly IMinioService _minioService = minioService;
    private readonly ITripsImageService _tripsImageService = tripsImageService;
    private readonly IMapper _mapper = mapper;

    public async Task<(string Location, TripImage TripImage)> UploadTripImageAsync(string tripId, TripImageUploadDto fileDto)
    {
        var tripIdLower = tripId.ToLowerInvariant();
        //TODO: add retries if minio succeeds but database fails
        var objectName = fileDto.FileName;

        var exists = await _minioService.BucketExistsAsync(tripIdLower);
        if (!exists)
        {
            await _minioService.MakeBucketAsync(tripIdLower);
            await _minioService.SetBucketPolicyToPublicAsync(tripIdLower);
        }

        using var stream = new MemoryStream(fileDto.Content);
        await _minioService.PutObjectStreamAsync(tripIdLower, objectName, stream, fileDto);

        var tripImage = new TripImage
        {
            TripId = Guid.Parse(tripIdLower),
            Url = $"http://localhost:9000/{tripIdLower}/{fileDto.FileName}",
            Caption = ""
        };

        await _tripsImageService.CreateTripImage(tripImage);

        var location = $"api/trips/{tripIdLower}/images/{tripImage.Id}";
        return (location, tripImage);
    }
}
