using Minio.DataModel.Response;
using Minio.DataModel.Result;
using Travel.Domain.Interfaces;
using Travel.Model;

namespace Travel.Domain.Services;

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
    
    public async Task<PutObjectResponse> PutObjectStreamAsync(string bucketName, string objectName, Stream stream, TripImageUploadDto fileDto)
    {
        return await _minioRepository.PutObjectStreamAsync(bucketName, objectName, stream, fileDto);
    }

    public async Task<ListAllMyBucketsResult> ListBucketsAsync()
    {
        return await _minioRepository.ListBucketsAsync();
    }
}