using Minio;
using Minio.DataModel.Args;
using Minio.DataModel.Response;
using Minio.DataModel.Result;
using Travel.Application.Interfaces;
using Travel.Model;

namespace Travel.Infrastructure.Repositories;

public class MinioRepository(IMinioClient minioClient) : IMinioRepository
{
    private readonly IMinioClient _minioClient = minioClient;

    public async Task SetBucketPolicyAsync(string bucketName, string policy)
    {
        await minioClient.SetPolicyAsync(new SetPolicyArgs()
            .WithBucket(bucketName)
            .WithPolicy(policy));
    }

    public async Task SetBucketPolicyToPublicAsync(string bucketName)
    {
        var policy = $@"
{{
  ""Version"": ""2012-10-17"",
  ""Statement"": [
    {{
      ""Effect"": ""Allow"",
      ""Principal"": {{ ""AWS"": [""*""] }},
      ""Action"": [""s3:GetObject""],
      ""Resource"": [""arn:aws:s3:::{bucketName}/*""]
    }}
  ]
}}";
        await SetBucketPolicyAsync(bucketName, policy);
    }

    public async Task MakeBucketAsync(string bucketName)
    {
        await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(bucketName));
    }

    public async Task<bool> BucketExistsAsync(string bucketName)
    {
        return await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucketName));
    }

    public async Task<PutObjectResponse> PutObjectStreamAsync(string bucketName, string objectName, Stream stream,
        TripImageUploadDto fileDto)
    {
        return await PutObject(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(stream)
            .WithObjectSize(fileDto.Content.Length)
            .WithContentType(fileDto.ContentType));
    }

    public async Task<PutObjectResponse> PutObjectLocalAsync(string bucketName, string objectName, string filePath,
        string contentType = "image/jpeg")
    {
        return await PutObject(new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithFileName(filePath)
            .WithContentType(contentType));
    }

    private async Task<PutObjectResponse> PutObject(PutObjectArgs putObjectArgs)
    {
        return await _minioClient.PutObjectAsync(putObjectArgs);
    }

    public async Task<ListAllMyBucketsResult> ListBucketsAsync()
    {
        return await _minioClient.ListBucketsAsync();
    }
}