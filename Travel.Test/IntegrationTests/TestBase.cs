using Microsoft.Extensions.DependencyInjection;
using Travel.Infrastructure.Context;
using Travel.Test.Helpers;

namespace Travel.Test.IntegrationTests;

[TestClass]
public class TestBase
{
    public static TravelWebApplicationFactory factory = null!;
    public static HttpClient client;
    
    
    [ClassInitialize(InheritanceBehavior.BeforeEachDerivedClass)]
    public static async Task ClassInitialize(TestContext context)
    {
        factory = new TravelWebApplicationFactory();
        await factory.InitializeDatabaseAsync();
        await factory.InitializeMinioAsync();
        
        context.WriteLine($"Minio Connections: {factory.minioContainer.GetConnectionString()}");
        
        client = factory.CreateClient();
    }

    public static async Task AddTripImageToDelete()
    {
        using var scope = factory.Services.CreateScope();
        var tripsDbContext = scope.ServiceProvider.GetRequiredService<TripsDbContext>();
        await TripsSeeder.SeedTripImageToDelete(tripsDbContext);
    }
}