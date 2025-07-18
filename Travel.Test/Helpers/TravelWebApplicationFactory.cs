using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;
using Travel.Infrastructure.Context;

namespace Travel.Test.Helpers;

public class TravelWebApplicationFactory : WebApplicationFactory<Program>
{
    public PostgreSqlContainer databaseContainer;
    
    public TravelWebApplicationFactory()
    {
        databaseContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15.2")
            .Build();
    }
    
    public async Task InitializeAsync()
    {
        await databaseContainer.StartAsync();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(TripsDbContext));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == 
                     typeof(DbContextOptions<TripsDbContext>));

            services.Remove(dbContextDescriptor);
            
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
        }
        base.Dispose(disposing);
    }
}