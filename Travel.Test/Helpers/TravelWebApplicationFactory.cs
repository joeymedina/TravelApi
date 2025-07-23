using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Testcontainers.PostgreSql;
using Travel.Infrastructure.Context;
using Testcontainers.Minio;
using Travel.Domain.Interfaces;

namespace Travel.Test.Helpers;
using Travel.Test.Extensions;
public class TravelWebApplicationFactory : WebApplicationFactory<Program>
{
    public PostgreSqlContainer databaseContainer;
    public MinioContainer minioContainer;
    public TravelWebApplicationFactory()
    {
        databaseContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15.2")
            .Build();

        minioContainer = new MinioBuilder().Build();
    }
    
    public async Task InitializeDatabaseAsync()
    {
        await databaseContainer.StartAsync();
    }
    
    public async Task InitializeMinioAsync()
    {
        await minioContainer.StartAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Remove<TripsDbContext>();
            services.Remove<DbContextOptions<TripsDbContext>>();
            services.Remove<IMinioClient>();
            
            services.AddMinio(configureClient => configureClient
                .WithEndpoint($"{minioContainer.Hostname}:{minioContainer.GetMappedPublicPort(MinioBuilder.MinioPort)}")
                .WithCredentials(minioContainer.GetAccessKey(), minioContainer.GetSecretKey())
                .WithSSL(false)
                .Build());
            
            services.AddDbContext<TripsDbContext>((container, options) =>
            {
                options.UseNpgsql(databaseContainer.GetConnectionString());
            });
        });

        builder.UseEnvironment("Development");
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            databaseContainer?.DisposeAsync();
            minioContainer?.DisposeAsync();
        }
        base.Dispose(disposing);
    }
}