using Bogus;
using Microsoft.EntityFrameworkCore;
using Travel.Model;

namespace Travel.Infrastructure.Context;

public static class TripsSeeder
{
    public static async Task SeedAsync(TripsDbContext context)
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
