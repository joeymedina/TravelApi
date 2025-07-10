using AutoMapper;
using Microsoft.AspNetCore.Http;
using Travel.Domain.Interfaces;
using Travel.Model;

namespace Travel.Domain.UseCases;

public class UploadTripImageUseCase(
    IMinioService minioService,
    ITripsImageService tripsImageService,
    IMapper mapper) : IUploadTripImageUseCase
{
    private readonly IMinioService _minioService = minioService;
    private readonly ITripsImageService _tripsImageService = tripsImageService;
    private readonly IMapper _mapper = mapper;

    public async Task<(string Location, TripImage TripImage)> UploadTripImageAsync(string tripId, IFormFile file)
    {
        //TODO: add retries if minio succeeds but database fails
        var objectName = file.FileName;

        var exists = await _minioService.BucketExistsAsync(tripId);
        if (!exists)
        {
            await _minioService.MakeBucketAsync(tripId);
            await _minioService.SetBucketPolicyToPublicAsync(tripId);
        }

        await using var stream = file.OpenReadStream();
        await _minioService.PutObjectStreamAsync(tripId, objectName, stream, file);    
        
        var tripImage = new TripImage 
        { 
            TripId = Guid.Parse(tripId), 
            Url = $"http://localhost:9000/{tripId}/{file.FileName}",  
            Caption = ""
        };
        
        await _tripsImageService.CreateTripImage(tripImage);
        
        var location = $"api/trips/{tripId}/images/{tripImage.Id}";
        return (location, tripImage);
    }
}

