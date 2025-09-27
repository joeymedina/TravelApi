using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;
using Travel.Application.Interfaces;
using Travel.Infrastructure.Context;
using Travel.Infrastructure.Repositories;
using Travel.Model;

namespace Travel.Infrastructure.Extensions;

public static class InfrastructureRegistrations
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(configuration.GetSection("Minio"));

        services.AddSingleton<IMinioClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MinioOptions>>().Value;

            return new MinioClient()
                .WithEndpoint(options.Endpoint)
                .WithCredentials(options.AccessKey, options.SecretKey)
                .WithSSL(false)
                .Build();
        });
        
        var postgresUser = configuration["Postgres:User"];
        var postgresPassword = configuration["Postgres:Password"];
        var postgresDatabase = configuration["Postgres:Database"];
        var postgresPort = configuration["Postgres:Port"] ?? "5433";
        var postgresServer = configuration["Postgres:Host"] ?? "localhost";
        
        services.AddDbContext<TripsDbContext>(options => options.UseNpgsql($"Server={postgresServer};Port={postgresPort};User Id={postgresUser};Password={postgresPassword};Database={postgresDatabase}"),ServiceLifetime.Transient);
        services.AddScoped<ITripsRepository, TripsRepository>();
        services.AddScoped<ITripsImageRepository, TripsImageRepository>();
        services.AddTransient<IMinioRepository, MinioRepository>();

        return services;
    }
}