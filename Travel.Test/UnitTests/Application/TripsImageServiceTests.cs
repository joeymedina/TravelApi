using Moq;
using Travel.Application.Interfaces;
using Travel.Application.Services;
using Travel.Domain.Entities;
using Travel.Model;
using Guid = Travel.Domain.Extensions.Guid;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Travel.Test.UnitTests.Application;

[TestClass]
public class TripsImageServiceTests
{
    private Mock<ITripsImageRepository> _mockRepo;
    private TripsImageService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<ITripsImageRepository>();
        _service = new TripsImageService(_mockRepo.Object);
    }

    [TestMethod]
    public async Task GetTripImages_ReturnsImages()
    {
        var tripId = Guid.NewGuidAsString();
        var images = new List<TripImageEntity> { new TripImageEntity { Id = Guid.NewGuid() }, new TripImageEntity { Id = Guid.NewGuid() } };
        _mockRepo.Setup(r => r.GetTripImages(tripId)).ReturnsAsync(images);
        var result = await _service.GetTripImages(tripId);
        CollectionAssert.AreEqual(images, result);
    }

    [TestMethod]
    public async Task GetTripImage_ReturnsImage()
    {
        var image = new TripImageEntity { Id = Guid.NewGuid() };
        var tripId = Guid.NewGuidAsString();
        var imageId = image.Id.ToString();
        _mockRepo.Setup(r => r.GetTripImage(tripId, imageId)).ReturnsAsync(image);
        var result = await _service.GetTripImage(tripId, imageId);
        Assert.AreEqual(image, result);
    }

    [TestMethod]
    public async Task CreateTripImage_CallsRepository()
    {
        var image = new TripImageEntity { Id = Guid.NewGuid() };
        _mockRepo.Setup(r => r.CreateTripImage(image)).Returns(Task.CompletedTask);
        await _service.CreateTripImage(image);
        _mockRepo.Verify(r => r.CreateTripImage(image), Times.Once);
    }

    [TestMethod]
    public async Task DeleteTripImage_CallsRepository()
    {
        var imageId = Guid.NewGuidAsString();
        await _service.DeleteTripImageAsync(imageId);
        _mockRepo.Verify(r => r.DeleteTripImageAsync(imageId), Times.Once);
    }

    [TestMethod]
    [Ignore("This needs to be fixed")]
    public async Task DeleteTripImages_DeletesAllImages()
    {
        var tripId = Guid.NewGuidAsString();
        var images = new List<TripImageEntity> { new TripImageEntity { Id = Guid.NewGuid() }, new TripImageEntity { Id = Guid.NewGuid() } };
        _mockRepo.Setup(r => r.GetTripImages(tripId)).ReturnsAsync(images);
        await _service.DeleteTripImagesAsync(tripId);
        _mockRepo.Verify(r => r.DeleteTripImageAsync(It.Is<string>(id => id == images[0].Id.ToString() || id == images[1].Id.ToString())), Times.Exactly(2));
    }

    [TestMethod]
    public async Task UpdateTripImage_CallsRepository()
    {
        var image = new TripImageEntity { Id = Guid.NewGuid() };
        _mockRepo.Setup(r => r.UpdateTripImage(image)).Returns(Task.CompletedTask);
        await _service.UpdateTripImage(image);
        _mockRepo.Verify(r => r.UpdateTripImage(image), Times.Once);
    }
}

