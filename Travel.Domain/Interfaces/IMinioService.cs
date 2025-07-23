using Minio.DataModel.Response;
using Minio.DataModel.Result;
using Travel.Model;

namespace Travel.Domain.Interfaces;

public interface IMinioService
{
    public Task MakeBucketAsync(string bucketName);
    public Task<bool> BucketExistsAsync(string bucketName);
    public Task<PutObjectResponse> PutObjectStreamAsync(string bucketName, string objectName, Stream stream,
        TripImageUploadDto fileDto);
    public Task<PutObjectResponse> PutObjectLocalAsync(string bucketName, string objectName, string filePath,
        string contentType = "image/jpeg");
    public Task<ListAllMyBucketsResult> ListBucketsAsync();
    public Task SetBucketPolicyToPublicAsync(string bucketName);

}