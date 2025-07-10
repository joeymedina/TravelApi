using Microsoft.Extensions.DependencyInjection;
using Minio.DataModel;
using Travel.Domain.Interfaces;
using Travel.Domain.Services;
using Travel.Domain.UseCases;

namespace Travel.Domain.Extensions;

public static class DomainRegistrations
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<ITripsService, TripsService>();
        services.AddTransient<ITripsImageService, TripsImageService>();
        services.AddTransient<IMinioService, MinioService>();
        services.AddTransient<IUploadTripImageUseCase, UploadTripImageUseCase>();
        return services;
    }
}
