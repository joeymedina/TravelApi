using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Travel.Domain.Interfaces;
using Travel.Infrastructure.Context;
using Travel.Infrastructure.Repositories;

namespace Travel.Infrastructure.Extensions;

public static class InfrastructureRegistrations
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var minioAccessKey = configuration["Minio:AccessKey"];
        var minioSecretKey = configuration["Minio:SecretKey"];
        var minioEndpoint = configuration["Minio:Endpoint"];
        services.AddMinio(configureClient => configureClient
            .WithEndpoint(minioEndpoint)
            .WithCredentials(minioAccessKey, minioSecretKey)
            .WithSSL(false)
            .Build());

        var postgresUser = configuration["Postgres:User"];
        var postgresPassword = configuration["Postgres:Password"];
        var postgresDatabase = configuration["Postgres:Database"];
        services.AddDbContext<TripsDbContext>(options => options.UseNpgsql($"Server=localhost;Port=5433;User Id={postgresUser};Password={postgresPassword};Database={postgresDatabase}"));
        services.AddScoped<ITripsRepository, TripsRepository>();
        services.AddScoped<ITripsImageRepository, TripsImageRepository>();
        services.AddTransient<IMinioRepository, MinioRepository>();

        return services;
    }
}