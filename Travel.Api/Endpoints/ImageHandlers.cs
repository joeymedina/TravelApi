using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Travel.Api.DTOs;
using Travel.Domain.Interfaces;
using Travel.Model;


namespace Travel.Api.Endpoints;

internal static class ImageHandlers
{
    private static List<TripImage> _tripImages =
    [
        new TripImage
        {
            Id = Guid.Parse("10AAC809-C188-458D-A12E-1AFC92E2FEE3"),
            Url = "http://google.com/myimage.png",
            Caption = "This is my image",
            TripId = Guid.Parse("BB68B434-3026-41E1-B253-97BA308D764F")
        },
        new TripImage
        {
            Id = Guid.Parse("10AAC809-C188-458D-A12E-1AFC92E2FEE2"),
            Url = "http://google.com/myimage2.png",
            Caption = "This is my second image",
            TripId = Guid.Parse("BB68B434-3026-41E1-B253-97BA308D764F")
        },
        new TripImage
        {
            Id = Guid.Parse("9AD58C86-AF81-4B97-BEA5-0218E5DC9E9B"),
            Url = "http://google.com/myimage3.png",
            Caption = "This is my third image",
            TripId = Guid.Parse("2358D025-1796-4A99-B527-D59120930E25")
        }
    ];

    public static async Task<List<TripImage>?> GetImages(string id, ITripsImageService tripsImageService)
    {
        return await tripsImageService.GetTripImages(id);
        // var result = _tripImages.Where(x => x.TripId == Guid.Parse(id)).ToList();
        // return result;
    }

    public static async Task<TripImage?> GetImage(string tripId, string id, ITripsImageService tripsImageService)
    {
        // return  _tripImages.FirstOrDefault(x => x.TripId == Guid.Parse(tripId) && x.Id == Guid.Parse(id));
        return await tripsImageService.GetTripImage(tripId, id);
    }
    
    public static async Task<Created<TripImage>> PostImage(string tripId, CreateTripImageDto tripImageDto, IMapper mapper, ITripsImageService tripsImageService)
    {
        var tripImage = mapper.Map<TripImage>(tripImageDto);  
        tripImage.TripId = Guid.Parse(tripId);
        await tripsImageService.CreateTripImage(tripImage);
        return TypedResults.Created($"api/trips/{tripId}/images/{tripImage.Id}", tripImage);
        // _tripImages.Add(tripImage);
        // return TypedResults.Created("", tripImage);
    }
    public static async Task<IResult> PatchImage(string tripId, string id, PatchTripImageDto tripImageDto, IMapper mapper, ITripsImageService tripsImageService)
    {
        var existingTripImage = await tripsImageService.GetTripImage(tripId, id);
        if (existingTripImage == null)
            return Results.NotFound();

        mapper.Map(tripImageDto, existingTripImage); 
        await tripsImageService.UpdateTripImage(existingTripImage);
        return Results.NoContent();
        // DeleteTripImage(tripId, id);
        // _tripImages.Add(tripImage);
        // return TypedResults.Created("",tripImage);
    }
    
    public static async Task DeleteTripImage(string tripId, string id, ITripsImageService tripImageService)
    {
        await tripImageService.DeleteTripImageAsync(id);
        // var image = GetImage(tripId, id);
        // if (image != null)
        // {
        //     _tripImages.Remove(image);
        // }
    }
    
    public static async Task<IResult> UploadImages([FromForm] IFormFileCollection files, string tripId, IUploadTripImageUseCase uploadTripImageUseCase)
    {
        if (files == null || files.Count == 0)
            return Results.BadRequest("No file uploaded.");
        
        var uploadedImages = new List<TripImage>();

        foreach (var file in files)
        {
            var fileDto = new TripImageUploadDto
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = await GetBytesAsync(file)
            };
            var result = await uploadTripImageUseCase.UploadTripImageAsync(tripId, fileDto);
            if (result.TripImage != null)
            {
                uploadedImages.Add(result.TripImage);
            }
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