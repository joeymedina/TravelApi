using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Minio.DataModel.Response;
using Travel.Application.Interfaces;
using Travel.Domain.Entities;
using Travel.Model.TripImage;

namespace Travel.Api.Endpoints;

internal static class ImageHandlers
{
    public static async Task<List<TripImage>?> GetImages(string id, IMapper mapper, ITripsImageService tripsImageService)
    {
        var tripImageEntities = await tripsImageService.GetTripImages(id);
        var tripImages = mapper.Map<List<TripImage>>(tripImageEntities);
        return tripImages;
    }

    public static async Task<TripImage?> GetImage(string tripId, string id, IMapper mapper, ITripsImageService tripsImageService)
    {
        var tripImageEntity = await tripsImageService.GetTripImage(tripId, id);
        var tripImage = mapper.Map<TripImage>(tripImageEntity);
        return tripImage;
    }
    
    public static async Task<Created<TripImage>> PostImage(string tripId, TripImageCreated tripImageDto, IMapper mapper, ITripsImageService tripsImageService)
    {
        var tripImageEntity = mapper.Map<TripImageEntity>(tripImageDto);  
        tripImageEntity.TripId = Guid.Parse(tripId);
        await tripsImageService.CreateTripImage(tripImageEntity);
        var tripImage = mapper.Map<TripImage>(tripImageEntity);
        
        return TypedResults.Created($"api/trips/{tripId}/images/{tripImage.Id}", tripImage);
    }
    public static async Task<IResult> PatchImage(string tripId, string id, TripImageUpdated tripImageDto, IMapper mapper, ITripsImageService tripsImageService)
    {
        var existingTripImage = await tripsImageService.GetTripImage(tripId, id);
        if (existingTripImage == null)
            return Results.NotFound();

        mapper.Map(tripImageDto, existingTripImage); 
        await tripsImageService.UpdateTripImage(existingTripImage);
        return Results.NoContent();
    }
    
    public static async Task DeleteTripImage(string tripId, string id, ITripsImageService tripImageService)
    {
        await tripImageService.DeleteTripImageAsync(id);
    }
    
    public static async Task<IResult> UploadImages([FromForm] IFormFileCollection files, string tripId, IMapper mapper, IUploadTripImageUseCase uploadTripImageUseCase)
    {
        if (files == null || files.Count == 0)
            return Results.BadRequest("No file uploaded.");
        
        var uploadedImages = new List<TripImage>();

        foreach (var file in files)
        {
            var fileDto = new TripImageUpload
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = await GetBytesAsync(file)
            };
            var result = await uploadTripImageUseCase.UploadTripImageAsync(tripId, fileDto);

            var trip = mapper.Map<TripImage>(result.TripImage);
            uploadedImages.Add(trip);

        }
        return TypedResults.Created("", uploadedImages);
        
    }
    public static async Task<PutObjectResponse> UploadLocal(IMinioService minioService)
    {
        // debug
        //var list = await minioService.ListBucketsAsync();
        const string bucketName = "tripimages";
        const string objectName = "island.jpeg";
        const string filePath = "/Users/joeymedina/minio/photos/island.jpeg";
        const string contentType = "image/jpeg";
        return  await minioService.PutObjectLocalAsync(bucketName, objectName, filePath, contentType);
    }

    private static async Task<byte[]> GetBytesAsync(IFormFile file)
    {
        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        return ms.ToArray();
    }
}