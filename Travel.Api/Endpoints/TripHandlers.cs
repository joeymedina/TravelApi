using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Minio;
using Travel.Application.Interfaces;
using Travel.Domain.Entities;
using Travel.Model.Trip;

namespace Travel.Api.Endpoints;

internal static class TripHandlers
{
    public static async Task GetBuckets(IMinioClient minioClient)
    {
        var getListBucketsTask = await minioClient.ListBucketsAsync();
        foreach (var bucket in getListBucketsTask.Buckets)
        {
            Console.WriteLine(bucket.Name + " " + bucket.CreationDateDateTime);
        }
    }

    public static List<Trip> GetTrips(IMapper mapper, ITripsService tripsService)
    {
        var tripEntities = tripsService.GetTrips();
        var trips = mapper.Map<List<Trip>>(tripEntities);
        
        return trips;
    }
    
    public static async Task<Trip?> GetTrip(string id, IMapper mapper, ITripsService tripsService)
    {
        var tripEntity = await tripsService.GetTrip(id);
        var trip = mapper.Map<Trip>(tripEntity);
        return trip;
    }

    public static async Task<Created<Trip>> PutTrip(string id, Trip trip, IMapper mapper, ITripsService tripsService)
    {
        var trip1 = await tripsService.GetTrip(id);
        var tripEntity = mapper.Map<TripEntity>(trip);
        return TypedResults.Created("",trip);
    }

    public static async Task<IResult> PatchTrip(string id, TripUpdated dto, IMapper mapper, ITripsService tripsService)
    {
        var existingTrip = await tripsService.GetTrip(id);
        if (existingTrip == null)
            return Results.NotFound();

        mapper.Map(dto, existingTrip); 
        await tripsService.UpdateTrip(existingTrip);
        return Results.NoContent();
    }
    public static async Task<Created<Trip>> PostTrip(TripCreated tripDto, IMapper mapper, ITripsService tripsService)
    {
        var tripEntity = mapper.Map<TripEntity>(tripDto);
        await tripsService.CreateTrip(tripEntity);
        var trip = mapper.Map<Trip>(tripEntity);
        
        return TypedResults.Created($"api/trips/{trip.Id}", trip);
    }

    public static async Task<IResult> DeleteTrip(string id, ITripsService tripsService, ITripsImageService imageService)
    {
        await imageService.DeleteTripImagesAsync(id);
        await tripsService.DeleteTripAsync(id);
        return Results.NoContent();
    }
}