using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;

namespace Travel.Infrastructure.Extensions;

public static class InfrastructureRegistrations
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var accessKey = configuration["Minio:AccessKey"];
        var secretKey = configuration["Minio:SecretKey"];
        var endpoint = configuration["Minio:Endpoint"];
        services.AddMinio(configureClient => configureClient
            .WithEndpoint(endpoint)
            .WithCredentials(accessKey, secretKey)
            .Build());

        services.AddDbContext<TripsDbContext>(options => options.UseNpgsql("Server=localhost;Port=5433;User Id=;Password=;Database=travel"));
        
        return services;
    }
}