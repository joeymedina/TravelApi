using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Minio.DataModel.Response;
using Moq;
using Travel.Domain.Interfaces;
using Travel.Domain.UseCases;
using Travel.Model;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

namespace Travel.Test.UnitTests.Domain;

[TestClass]
public class UploadTripImageUseCaseTests
{
    private Mock<IMinioService> _mockMinioService;
    private Mock<ITripsImageService> _mockTripsImageService;
    private Mock<IMapper> _mockMapper;
    private UploadTripImageUseCase _useCase;
    private string _tripId;
    private string _fileName;
    private Mock<IFormFile> _mockFile;

    [TestInitialize]
    public void TestInitialize()
    {
        _mockMinioService = new Mock<IMinioService>();
        _mockTripsImageService = new Mock<ITripsImageService>();
        _mockMapper = new Mock<IMapper>();
        _useCase = new UploadTripImageUseCase(_mockMinioService.Object, _mockTripsImageService.Object, _mockMapper.Object);
        _tripId = Guid.NewGuid().ToString();
        _fileName = "test.jpg";
        _mockFile = new Mock<IFormFile>();
        _mockFile.Setup(f => f.FileName).Returns(_fileName);
        _mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[] { 1, 2, 3 }));
    }

    [TestMethod]
    public async Task UploadTripImageAsync_BucketDoesNotExist_CreatesBucketAndUploadsImage()
    {
        _mockMinioService.Setup(x => x.BucketExistsAsync(_tripId)).ReturnsAsync(false);
        _mockMinioService.Setup(x => x.MakeBucketAsync(_tripId)).Returns(Task.CompletedTask);
        _mockMinioService.Setup(x => x.SetBucketPolicyToPublicAsync(_tripId)).Returns(Task.CompletedTask);
        _mockMinioService.Setup(x => x.PutObjectStreamAsync(_tripId, _fileName, It.IsAny<Stream>(), It.IsAny<IFormFile>()))
            .ReturnsAsync(new PutObjectResponse(HttpStatusCode.Accepted, "key", new Dictionary<string,string>(), 0, "contentMd5"));

        var tripImage = new TripImage { TripId = Guid.Parse(_tripId), Url = $"http://localhost:9000/{_tripId}/{_fileName}", Caption = "" };
        _mockTripsImageService.Setup(x => x.CreateTripImage(It.IsAny<TripImage>())).Returns(Task.CompletedTask);

        var (location, resultImage) = await _useCase.UploadTripImageAsync(_tripId, _mockFile.Object);

        _mockMinioService.Verify(x => x.MakeBucketAsync(_tripId), Times.Once);
        _mockMinioService.Verify(x => x.SetBucketPolicyToPublicAsync(_tripId), Times.Once);
        _mockMinioService.Verify(x => x.PutObjectStreamAsync(_tripId, _fileName, It.IsAny<Stream>(), _mockFile.Object), Times.Once);
        _mockTripsImageService.Verify(x => x.CreateTripImage(It.IsAny<TripImage>()), Times.Once);
        Assert.AreEqual($"api/trips/{_tripId}/images/{resultImage.Id}", location);
        Assert.AreEqual(tripImage.TripId, resultImage.TripId);
        Assert.AreEqual(tripImage.Url, resultImage.Url);
    }

    [TestMethod]
    public async Task UploadTripImageAsync_BucketAlreadyExists_DoesNotCreateBucket()
    {
        _mockMinioService.Setup(x => x.BucketExistsAsync(_tripId)).ReturnsAsync(true);
        _mockMinioService.Setup(x => x.PutObjectStreamAsync(_tripId, _fileName, It.IsAny<Stream>(), It.IsAny<IFormFile>()))
            .ReturnsAsync(new PutObjectResponse(HttpStatusCode.Accepted, "key", new Dictionary<string,string>(), 0, "contentMd5"));
        _mockTripsImageService.Setup(x => x.CreateTripImage(It.IsAny<TripImage>())).Returns(Task.CompletedTask);

        var (location, resultImage) = await _useCase.UploadTripImageAsync(_tripId, _mockFile.Object);

        _mockMinioService.Verify(x => x.MakeBucketAsync(_tripId), Times.Never);
        _mockMinioService.Verify(x => x.SetBucketPolicyToPublicAsync(_tripId), Times.Never);
        _mockMinioService.Verify(x => x.PutObjectStreamAsync(_tripId, _fileName, It.IsAny<Stream>(), _mockFile.Object), Times.Once);
        _mockTripsImageService.Verify(x => x.CreateTripImage(It.IsAny<TripImage>()), Times.Once);
        Assert.AreEqual($"api/trips/{_tripId}/images/{resultImage.Id}", location);
    }

    [TestMethod]
    public async Task UploadTripImageAsync_TripImageCreationFails_ThrowsException()
    {
        _mockMinioService.Setup(x => x.BucketExistsAsync(_tripId)).ReturnsAsync(true);
        _mockMinioService.Setup(x => x.PutObjectStreamAsync(_tripId, _fileName, It.IsAny<Stream>(), It.IsAny<IFormFile>()))
            .ReturnsAsync(new PutObjectResponse(HttpStatusCode.Accepted, "key", new Dictionary<string,string>(), 0, "contentMd5"));
        _mockTripsImageService.Setup(x => x.CreateTripImage(It.IsAny<TripImage>())).ThrowsAsync(new Exception("DB error"));

        await Assert.ThrowsExceptionAsync<Exception>(async () =>
            await _useCase.UploadTripImageAsync(_tripId, _mockFile.Object));
    }

    [TestMethod]
    public async Task UploadTripImageAsync_MinioUploadFails_ThrowsException()
    {
        _mockMinioService.Setup(x => x.BucketExistsAsync(_tripId)).ReturnsAsync(true);
        _mockMinioService.Setup(x => x.PutObjectStreamAsync(_tripId, _fileName, It.IsAny<Stream>(), It.IsAny<IFormFile>()))
            .ThrowsAsync(new Exception("Minio error"));
        _mockTripsImageService.Setup(x => x.CreateTripImage(It.IsAny<TripImage>())).Returns(Task.CompletedTask);

        await Assert.ThrowsExceptionAsync<Exception>(async () =>
            await _useCase.UploadTripImageAsync(_tripId, _mockFile.Object));
    }

    [TestMethod]
    public async Task UploadTripImageAsync_NullFile_ThrowsArgumentNullException()
    {
        await Assert.ThrowsExceptionAsync<NullReferenceException>(async () =>
            await _useCase.UploadTripImageAsync(_tripId, null!));
    }
}
