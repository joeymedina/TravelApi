using Microsoft.AspNetCore.Http;
using Minio.DataModel.Response;
using Minio.DataModel.Result;

namespace Travel.Domain.Interfaces;

public interface IMinioUseCase
{
    public Task MakeBucketAsync(string bucketName);
    public Task<bool> BucketExistsAsync(string bucketName);
    public Task<PutObjectResponse> PutObjectStreamAsync(string bucketName, string objectName, Stream stream,
        IFormFile file);
    public Task<PutObjectResponse> PutObjectLocalAsync(string bucketName, string objectName, string filePath,
        string contentType = "image/jpeg");
    public Task<ListAllMyBucketsResult> ListBucketsAsync();
    
}