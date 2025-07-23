using System.Net;
using System.Net.Http.Json;
using Travel.Api.DTOs;
using Travel.Model;

namespace Travel.Test.IntegrationTests;

[TestClass]
[DoNotParallelize]
public class TripHandlersTests : TestBase
{
    [TestMethod]
    public async Task GetTrips_Should_ReturnTrips()
    {
        var response = await client.GetAsync("/api/trips");
        var content = await response.Content.ReadFromJsonAsync<List<Trip>>();
        
        Assert.IsTrue(content?.Count > 0, "Expected to return at least one trip");
    }

    [TestMethod]
    public async Task GetTrip_Should_ReturnTrip()
    {
        var tripId = "BB68B434-3026-41E1-B253-97BA308D764F";
        var response = await client.GetAsync($"/api/trips/{tripId}");
        var trip = await response.Content.ReadFromJsonAsync<Trip>();
        Assert.IsNotNull(trip);
        Assert.AreEqual("BB68B434-3026-41E1-B253-97BA308D764F", trip.Id.ToString().ToUpper(), "Expected trip ID to match");
    }

    [TestMethod]
    public async Task PostTrip_Should_CreateTrip()
    {
        CreateTripDto createTripDto = new CreateTripDto
        {
            Title = "Test Trip",
            Location = "Test Location",
            StartDate = DateTime.Parse("2023-01-01"),
            EndDate = DateTime.Parse("2023-01-03"),
            Story = "This is a test trip"
        };
            
        HttpContent messageContent = JsonContent.Create(createTripDto);
        
        var response = await client.PostAsync("/api/trips/", messageContent);
        var trip = await response.Content.ReadFromJsonAsync<Trip>();
        
        Assert.IsNotNull(trip);
        AssertTripsAreEqual(trip, createTripDto);
        
        var getTripResponse = await client.GetAsync($"/api/trips/{trip.Id}");
        var tripFromDatabase = await getTripResponse.Content.ReadFromJsonAsync<Trip>();
        
        Assert.IsNotNull(tripFromDatabase);
        AssertTripsAreEqual(tripFromDatabase, createTripDto);
    }

    [TestMethod]
    public async Task PatchTrip_Should_Return404_WhenInvalidTripId()
    {
        PatchTripDto patchTrip = new PatchTripDto
        {
            Title = "Test Trip",
        }; 
        var badTripId = Guid.Parse("AA68B434-3026-41E1-B253-97BA308D764F");
        HttpContent messageContent = JsonContent.Create(patchTrip);
        
        var response = await client.PatchAsync($"/api/trips/{badTripId}", messageContent);
        Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode, "Expected 404 Not Found for non-existing trip");
    }

    [TestMethod]
    public async Task PatchTrip_Should_UpdateTripWhenValid()
    {
        PatchTripDto patchTrip = new PatchTripDto
        {
            Title = "Updated Trip",
            Story = "Updated story"
        };
        var existingTripId = "BB68B434-3026-41E1-B253-97BA308D764F";
        HttpContent messageContent = JsonContent.Create(patchTrip);
        
        var response = await client.PatchAsync($"/api/trips/{existingTripId}", messageContent);
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);

        var getTripResponse = await client.GetAsync($"/api/trips/{existingTripId}");
        var updatedTrip = await getTripResponse.Content.ReadFromJsonAsync<Trip>();
        Assert.IsNotNull(updatedTrip);
        AssertTripsAreEqual(updatedTrip, patchTrip);
    }

    [TestMethod]
    public async Task DeleteTrip_Should_DeleteTrip()
    {
        var deleteTripId = "2358D025-1796-4A99-B527-D59120930E25";
        var response = await client.DeleteAsync($"/api/trips/{deleteTripId}");
        Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode, "Expected 204 No Content for successful deletion");
        var getTripResponse = await client.GetAsync($"/api/trips/{deleteTripId}");
        var content = await getTripResponse.Content.ReadFromJsonAsync<Trip?>();
        Assert.IsNull(content, "Expected empty content for deleted trip");
    }
    
    private static void AssertTripsAreEqual(Trip trip, CreateTripDto createTripDto)
    {
        Assert.AreEqual(createTripDto.Title, trip?.Title);
        Assert.AreEqual(createTripDto.Location, trip?.Location);
        Assert.AreEqual(createTripDto.StartDate, trip?.StartDate);
        Assert.AreEqual(createTripDto.EndDate, trip?.EndDate);
        Assert.AreEqual(createTripDto.Story, trip?.Story);
    }

    private static void AssertTripsAreEqual(Trip trip, PatchTripDto patchTripDto)
    {
        if (patchTripDto.Title != null)
            Assert.AreEqual(patchTripDto.Title, trip?.Title);
        if (patchTripDto.Location != null)
            Assert.AreEqual(patchTripDto.Location, trip?.Location);
        if (patchTripDto.StartDate != null)
            Assert.AreEqual(patchTripDto.StartDate, trip?.StartDate);
        if (patchTripDto.EndDate != null)
            Assert.AreEqual(patchTripDto.EndDate, trip?.EndDate);
        if (patchTripDto.Story != null)
            Assert.AreEqual(patchTripDto.Story, trip?.Story);
    }
}