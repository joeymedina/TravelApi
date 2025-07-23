using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Minio;
using Travel.Api.DTOs;
using Travel.Domain.Interfaces;
using Travel.Model;

namespace Travel.Api.Endpoints;

internal static class TripHandlers
{
    private static List<Trip> _trips = 
        [
            new Trip
            {
                Id = Guid.Parse("BB68B434-3026-41E1-B253-97BA308D764F"),
                Title = "First Trip To Vegas",
                Location = "Las Vegas, Nevada",
                StartDate = new DateTime(2025, 4, 17),
                EndDate = new DateTime(2025, 4, 21),
                Story = "Vegas was amazing",
                Images = [],
            },
            new Trip
            {
                Id = Guid.Parse("2358D025-1796-4A99-B527-D59120930E25"),
                Title = "Summer Lake",
                Location = "Gravois Mills, MO",
                StartDate = new DateTime(2025, 5, 23),
                EndDate = new DateTime(2025, 5, 26),
                Story = "I found my first artifact",
                Images = [],
            }
        ];
    
    public static async Task GetBuckets(IMinioClient minioClient)
    {
        var getListBucketsTask = await minioClient.ListBucketsAsync();
        foreach (var bucket in getListBucketsTask.Buckets)
        {
            Console.WriteLine(bucket.Name + " " + bucket.CreationDateDateTime);
        }
    }

    public static List<Trip> GetTrips(ITripsService tripsService)
    {
        // return _trips;
        return tripsService.GetTrips();
    }
    
    public static async Task<Trip?> GetTrip(string id, ITripsService tripsService)
    {
        return await tripsService.GetTrip(id);
        return _trips.FirstOrDefault(x => x.Id.ToString().Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public static async Task<Created<Trip>> PutTrip(string id, Trip trip, ITripsService tripsService)
    {
        var trip1 = await tripsService.GetTrip(id);
        _trips.Remove(trip1);
        _trips.Add(trip);
        return TypedResults.Created("",trip);
    }

    public static async Task<IResult> PatchTrip(string id, PatchTripDto dto, IMapper mapper, ITripsService tripsService)
    {
        var existingTrip = await tripsService.GetTrip(id);
        if (existingTrip == null)
            return Results.NotFound();

        mapper.Map(dto, existingTrip); 
        await tripsService.UpdateTrip(existingTrip);
        return Results.NoContent();
    }
    public static async Task<Created<Trip>> PostTrip(CreateTripDto tripDto, IMapper mapper, ITripsService tripsService)
    {
        var trip = mapper.Map<Trip>(tripDto);
        await tripsService.CreateTrip(trip);
        return TypedResults.Created($"api/trips/{trip.Id}", trip);
        // _trips.Add(trip);
        // return TypedResults.Created("",trip);
    }

    public static async Task<IResult> DeleteTrip(string id, ITripsService tripsService, ITripsImageService imageService)
    {
        await imageService.DeleteTripImagesAsync(id);
        await tripsService.DeleteTripAsync(id);
        return Results.NoContent();
        // var trip = await tripsService.GetTrip(id);
        // if (trip != null)
        // {
        //     _trips.Remove(trip);
        // }
    }
}