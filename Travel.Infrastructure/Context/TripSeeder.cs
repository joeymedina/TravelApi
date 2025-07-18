using Bogus;
using Microsoft.EntityFrameworkCore;
using Travel.Model;

namespace Travel.Infrastructure.Context;

public static class TripsSeeder
{
    private static List<Trip> Trips = 
    [
        new Trip
        {
            Id = Guid.Parse("BB68B434-3026-41E1-B253-97BA308D764F"),
            Title = "First Trip To Vegas",
            Location = "Las Vegas, Nevada",
            StartDate = new DateTime(2025, 4, 17, 0,0,0, DateTimeKind.Utc),
            EndDate = new DateTime(2025, 4, 21, 0,0,0, DateTimeKind.Utc),
            Story = "Vegas was amazing",
        },
        new Trip
        {
            Id = Guid.Parse("2358D025-1796-4A99-B527-D59120930E25"),
            Title = "Summer Lake",
            Location = "Gravois Mills, MO",
            StartDate = new DateTime(2025, 5, 23, 0,0,0, DateTimeKind.Utc),
            EndDate = new DateTime(2025, 5, 26, 0,0,0, DateTimeKind.Utc),
            Story = "I found my first artifact",
        }
    ];

    private static List<TripImage> TripImages =
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
    public static async Task SeedAsync(TripsDbContext context)
    {
        await context.Trips.AddRangeAsync(Trips);
        await context.TripImages.AddRangeAsync(TripImages);
        await context.SaveChangesAsync();
    }
    
    public static async Task SeedRandomDataAsync(TripsDbContext context)
    {
        if (await context.Trips.AnyAsync())
            return; // Already seeded

        var faker = new Faker("en");

        var trips = new List<Trip>();

        for (int i = 0; i < 5; i++)
        {
            var tripId = Guid.NewGuid();

            var trip = new Trip
            {
                Id = tripId,
                Title = faker.Lorem.Sentence(3),
                Location = faker.Address.City(),
                StartDate = faker.Date.Past(2).ToUniversalTime(),
                EndDate = faker.Date.Recent().ToUniversalTime(),
                Story = faker.Lorem.Paragraphs(2),
                Images = new List<TripImage>()
            };

            for (int j = 0; j < faker.Random.Int(1, 4); j++)
            {
                trip.Images.Add(new TripImage
                {
                    Id = Guid.NewGuid(),
                    Url = faker.Image.PicsumUrl(),
                    Caption = faker.Lorem.Sentence(5),
                    TripId = tripId
                });
            }

            trips.Add(trip);
        }

        await context.Trips.AddRangeAsync(trips);
        await context.SaveChangesAsync();
    }
}
