using Moq;
using Travel.Domain.Interfaces;
using Travel.Domain.Services;
using Travel.Model;
using Guid = Travel.Domain.Extensions.Guid;
namespace Travel.Test.UnitTests.Domain;

[TestClass]
public class TripsServiceTests
{
    private Mock<ITripsRepository> _mockRepo;
    private TripsService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<ITripsRepository>();
        _service = new TripsService(_mockRepo.Object);
    }

    [TestMethod]
    public async Task GetTrip_ReturnsTrip_WhenFound()
    {
        var trip = new Trip { Id = Guid.NewGuid(), Title = "Test Trip" };
        _mockRepo.Setup(r => r.GetTrip(trip.Id.ToString())).ReturnsAsync(trip);
        var result = await _service.GetTrip(trip.Id.ToString());
        Assert.AreEqual(trip, result);
    }

    [TestMethod]
    public async Task GetTrip_ReturnsNull_WhenNotFound()
    {
        _mockRepo.Setup(r => r.GetTrip(It.IsAny<string>())).ReturnsAsync((Trip?)null);
        var result = await _service.GetTrip(Guid.NewGuidAsString());
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetTrips_ReturnsListOfTrips()
    {
        var trips = new List<Trip> { new Trip { Title = "Trip1" }, new Trip { Title = "Trip2" } };
        _mockRepo.Setup(r => r.GetTrips()).Returns(trips);
        var result = _service.GetTrips();
        CollectionAssert.AreEqual(trips, result);
    }

    [TestMethod]
    public async Task CreateTrip_SetsDatesToUtc_AndCallsRepository()
    {
        var trip = new Trip { Title = "Trip", StartDate = DateTime.Now, EndDate = DateTime.Now.AddDays(1) };
        _mockRepo.Setup(r => r.CreateTrip(trip)).Returns(Task.CompletedTask);
        await _service.CreateTrip(trip);
        Assert.AreEqual(DateTimeKind.Utc, trip.StartDate.Kind);
        Assert.AreEqual(DateTimeKind.Utc, trip.EndDate.Kind);
        _mockRepo.Verify(r => r.CreateTrip(trip), Times.Once);
    }

    [TestMethod]
    public void DeleteTrip_CallsRepository()
    {
        var id = Guid.NewGuidAsString();
        _service.DeleteTrip(id);
        _mockRepo.Verify(r => r.DeleteTrip(id), Times.Once);
    }

    [TestMethod]
    public async Task UpdateTrip_CallsRepository()
    {
        var trip = new Trip { Title = "Trip" };
        _mockRepo.Setup(r => r.UpdateTrip(trip)).Returns(Task.CompletedTask);
        await _service.UpdateTrip(trip);
        _mockRepo.Verify(r => r.UpdateTrip(trip), Times.Once);
    }
}

