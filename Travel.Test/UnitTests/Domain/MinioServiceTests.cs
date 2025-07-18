using System.Net;
using Microsoft.AspNetCore.Http;
using Minio.DataModel.Response;
using Minio.DataModel.Result;
using Moq;
using Travel.Domain.Interfaces;
using Travel.Domain.Services;

namespace Travel.Test.UnitTests.Domain;

[TestClass]
public class MinioServiceTests
{
    private Mock<IMinioRepository> _mockRepo;
    private MinioService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepo = new Mock<IMinioRepository>();
        _service = new MinioService(_mockRepo.Object);
    }

    [TestMethod]
    public async Task SetBucketPolicyToPublicAsync_CallsRepository()
    {
        _mockRepo.Setup(r => r.SetBucketPolicyToPublicAsync("bucket")).Returns(Task.CompletedTask);
        await _service.SetBucketPolicyToPublicAsync("bucket");
        _mockRepo.Verify(r => r.SetBucketPolicyToPublicAsync("bucket"), Times.Once);
    }

    [TestMethod]
    public async Task MakeBucketAsync_CallsRepository()
    {
        _mockRepo.Setup(r => r.MakeBucketAsync("bucket")).Returns(Task.CompletedTask);
        await _service.MakeBucketAsync("bucket");
        _mockRepo.Verify(r => r.MakeBucketAsync("bucket"), Times.Once);
    }

    [TestMethod]
    public async Task BucketExistsAsync_ReturnsRepositoryResult()
    {
        _mockRepo.Setup(r => r.BucketExistsAsync("bucket")).ReturnsAsync(true);
        var result = await _service.BucketExistsAsync("bucket");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task PutObjectLocalAsync_ReturnsRepositoryResult()
    {

        var response = new PutObjectResponse(HttpStatusCode.Accepted, "key", new Dictionary<string,string>(), 0, "contentMd5");
        _mockRepo.Setup(r => r.PutObjectLocalAsync("bucket", "object", "path", "type")).ReturnsAsync(response);
        var result = await _service.PutObjectLocalAsync("bucket", "object", "path", "type");
        Assert.AreEqual(response, result);
    }

    [TestMethod]
    public async Task PutObjectStreamAsync_ReturnsRepositoryResult()
    {
        var response = new PutObjectResponse(HttpStatusCode.Accepted, "key", new Dictionary<string,string>(), 0, "contentMd5");
        var mockFile = new Mock<IFormFile>();
        var stream = new MemoryStream();
        _mockRepo.Setup(r => r.PutObjectStreamAsync("bucket", "object", stream, mockFile.Object)).ReturnsAsync(response);
        var result = await _service.PutObjectStreamAsync("bucket", "object", stream, mockFile.Object);
        Assert.AreEqual(response, result);
    }

    [TestMethod]
    public async Task ListBucketsAsync_ReturnsRepositoryResult()
    {
        var bucketsResult = new ListAllMyBucketsResult();
        _mockRepo.Setup(r => r.ListBucketsAsync()).ReturnsAsync(bucketsResult);
        var result = await _service.ListBucketsAsync();
        Assert.AreEqual(bucketsResult, result);
    }
}

