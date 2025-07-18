using System.Diagnostics;
using System.Net.Http.Json;
using Travel.Model;
using Travel.Test.Helpers;

namespace Travel.Test.IntegrationTests;

[TestClass]
public class TripHandlersTests
{
    [TestMethod]
    public async Task GetTrips_Should_ReturnTrips()
    {
        TravelWebApplicationFactory factory = new TravelWebApplicationFactory();
        await factory.InitializeAsync();
        using var client = factory.CreateClient();
        var response = await client.GetAsync("/api/trips");
        var content = await response.Content.ReadFromJsonAsync<List<Trip>>();
        
        Assert.AreEqual(2, content?.Count);
    }
}