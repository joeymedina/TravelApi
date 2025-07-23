using System.Net;
using System.Net.Http.Json;
using System.Text;
using Travel.Api.DTOs;
using Travel.Model;

namespace Travel.Test.IntegrationTests;

[TestClass]
[DoNotParallelize]
public class ImageHandlersTests : TestBase
{
    
    [TestMethod]
    public async Task GetImage_ShouldReturnImage_WhenImageExists()
    {
        var imageId = "10AAC809-C188-458D-A12E-1AFC92E2FEE3";
        var tripId = "BB68B434-3026-41E1-B253-97BA308D764F";

        var response = await client.GetAsync($"/api/trips/{tripId}/images/{imageId}");

        response.EnsureSuccessStatusCode();
        var image = await response.Content.ReadFromJsonAsync<TripImage>();
        Assert.IsNotNull(image, "Expected to return an image");
    }

    [TestMethod]
    public async Task GetImage_ShouldReturnNotFound_WhenImageDoesNotExist()
    {
        // Arrange
        var nonExistentImageId = "30AAC809-C188-458D-A12E-1AFC92E2FEE3";
        var tripId = "BB68B434-3026-41E1-B253-97BA308D764F";

        // Act
        var response = await client.GetAsync($"/api/trips/{tripId}/images/{nonExistentImageId}");
        var image = await response.Content.ReadFromJsonAsync<TripImage?>();

        // Assert
        Assert.IsNull(image, "Expected to not return an image");
    }
    
    [TestMethod]
    public async Task GetImages_ShouldReturnImages_WhenTripHasImages()
    {
        var tripId = "BB68B434-3026-41E1-B253-97BA308D764F";

        var response = await client.GetAsync($"/api/trips/{tripId}/images");

        response.EnsureSuccessStatusCode();
        var images = await response.Content.ReadFromJsonAsync<List<TripImage?>>();
        Assert.IsNotNull(images, "Expected to return a list of images");
        Assert.IsTrue(images.Count > 0, "Expected to return at least one image");
    }
    
    [TestMethod]
    public async Task PostImage_ShouldCreateImage_WhenValidDataProvided()
    {
        var tripId = "BB68B434-3026-41E1-B253-97BA308D764F";
        var newImage = new CreateTripImageDto
        {
            Url = "http://example.com/newimage.png",
            Caption = "New Image Caption"
        };
        
        HttpContent messageContent = JsonContent.Create(newImage);

        var response = await client.PostAsync($"/api/trip/{tripId}/images", messageContent);
        
        response.EnsureSuccessStatusCode();
        var createdImage = await response.Content.ReadFromJsonAsync<TripImage>();
        
        Assert.IsNotNull(createdImage, "Expected to return the created image");
        Assert.AreEqual(newImage.Url, createdImage.Url, "Expected image URL to match");
        Assert.AreEqual(newImage.Caption, createdImage.Caption, "Expected image caption to match");
        
        var getTripImageResponse = await client.GetAsync($"/api/trips/{tripId}/images/{createdImage.Id.ToString()}");
        var tripImage = await getTripImageResponse.Content.ReadFromJsonAsync<TripImage>();
       
        Assert.IsNotNull(tripImage);
        Assert.AreEqual(newImage.Url, tripImage.Url, "Expected image URL to match");
        Assert.AreEqual(newImage.Caption, tripImage.Caption, "Expected image caption to match");
    }
    
    [TestMethod]
    public async Task PatchImage_ShouldUpdateImage_WhenValidDataProvided()
    {
        var tripId = "2358D025-1796-4A99-B527-D59120930E25";
        var imageId = "9AD58C86-AF81-4B97-BEA5-0218E5DC9E9B";
        
        var updatedImage = new PatchTripImageDto
        {
            Caption = "Updated Image Caption"
        };
        
        HttpContent messageContent = JsonContent.Create(updatedImage);

        var response = await client.PatchAsync($"/api/trips/{tripId}/images/{imageId}", messageContent);
        
        response.EnsureSuccessStatusCode();
        
        var getTripImageResponse = await client.GetAsync($"/api/trips/{tripId}/images/{imageId}");
        var tripImage = await getTripImageResponse.Content.ReadFromJsonAsync<TripImage>();
        
        Assert.IsNotNull(tripImage, "Expected to return the updated image");
        Assert.AreEqual(updatedImage.Caption, tripImage.Caption, "Expected image caption to match");
    }
    
    [TestMethod]
    public async Task DeleteImage_ShouldRemoveImage_WhenImageExists()
    {
        var tripId = "BB68B434-3026-41E1-B253-97BA308D764F";
        var imageId = "40AAC809-C188-458D-A12E-1AFC92E2FEE2";
        await AddTripImageToDelete();
        
        var getTripImageResponseVerify = await client.GetAsync($"/api/trips/{tripId}/images/{imageId}");
        Assert.AreEqual(HttpStatusCode.OK, getTripImageResponseVerify.StatusCode, "Expected to return OK status for image");
        var tripImageVerify = await getTripImageResponseVerify.Content.ReadFromJsonAsync<TripImage?>();
        Assert.IsNotNull(tripImageVerify, "Expected to return an image");
        
        var response = await client.DeleteAsync($"/api/trips/{tripId}/images/{imageId}");
        
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode, "Expected to return No Content status");

        var getTripImageResponse = await client.GetAsync($"/api/trips/{tripId}/images/{imageId}");
        Assert.AreEqual(HttpStatusCode.OK, getTripImageResponse.StatusCode, "Expected to return Not Found status for deleted image");
        var tripImage = await getTripImageResponse.Content.ReadFromJsonAsync<TripImage?>();
        Assert.IsNull(tripImage, "Expected to not return an image after deletion");
    }
    
    [TestMethod]
    public async Task UploadImage_ShouldUploadImage_WhenValidDataProvided()
    {
        var fileName = "testimage.png";
        var tripId = "BB68B434-3026-41E1-B253-97BA308D764F";
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", fileName);
        
        using var fileStream = new FileStream(filePath, FileMode.Open);
        using var content = new StreamContent(fileStream);
        content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        using var formData = new MultipartFormDataContent();
        formData.Add(content, "files", fileName);
        
        var response = await client.PostAsync($"/api/trip/{tripId}/upload", formData);
        response.EnsureSuccessStatusCode();
        
        var uploadedImage = await response.Content.ReadFromJsonAsync<List<TripImage>>();
        
        Assert.IsNotNull(uploadedImage, "Expected to return the uploaded image");
        Assert.AreEqual(Guid.Parse(tripId), uploadedImage[0].TripId, "Expected trip id of image to be provided trip id");
        Assert.IsTrue(uploadedImage[0].Url.Contains(fileName), "Expected image URL to contain the uploaded file name");
    }
}