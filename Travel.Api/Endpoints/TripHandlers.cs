using Microsoft.AspNetCore.Http.HttpResults;
using Minio;
using Minio.DataModel.Args;
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

    public static List<Trip> GetTrips()
    {
        return _trips;
    }
    
    public static Trip? GetTrip(string id)
    {
        return _trips.FirstOrDefault(x => x.Id.ToString().Equals(id, StringComparison.OrdinalIgnoreCase));
    }

    public static Created<Trip> PutTrip(string id, Trip trip)
    {
        var trip1 = GetTrip(id);
        _trips.Remove(trip1);
        _trips.Add(trip);
        return TypedResults.Created("",trip);
    }
    public static Created<Trip> PostTrip(Trip trip)
    {
        _trips.Add(trip);
        return TypedResults.Created("",trip);
    }

    public static void DeleteTrip(string id)
    {
        var trip = GetTrip(id);
        if (trip != null)
        {
            _trips.Remove(trip);
        }
    }
    
    
}