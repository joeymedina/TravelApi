using Minio.DataModel.Response;
using Minio.DataModel.Result;
using Travel.Application.Interfaces;
using Travel.Model.TripImage;

namespace Travel.Application.Services;

public class MinioService(IMinioRepository minioRepository) : IMinioService
{
    private readonly IMinioRepository _minioRepository = minioRepository;
    
    public async Task SetBucketPolicyToPublicAsync(string bucketName)
    {
        await _minioRepository.SetBucketPolicyToPublicAsync(bucketName);
    }
    public async Task MakeBucketAsync(string bucketName)
    {
        await _minioRepository.MakeBucketAsync(bucketName);
    }
    
    public async Task<bool> BucketExistsAsync(string bucketName)
    {
        return await _minioRepository.BucketExistsAsync(bucketName);
    }
    
    public async Task<PutObjectResponse> PutObjectLocalAsync(string bucketName, string objectName, string filePath, string contentType)
    {
        return await _minioRepository.PutObjectLocalAsync(bucketName, objectName, filePath, contentType);
    }
    
    public async Task<PutObjectResponse> PutObjectStreamAsync(string bucketName, string objectName, Stream stream, TripImageUpload file)
    {
        return await _minioRepository.PutObjectStreamAsync(bucketName, objectName, stream, file);
    }

    public async Task<ListAllMyBucketsResult> ListBucketsAsync()
    {
        return await _minioRepository.ListBucketsAsync();
    }
}