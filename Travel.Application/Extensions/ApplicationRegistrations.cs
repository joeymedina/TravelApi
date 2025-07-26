using Microsoft.Extensions.DependencyInjection;
using Travel.Application.Interfaces;
using Travel.Application.Services;
using Travel.Application.UseCases;

namespace Travel.Application.Extensions;

public static class ApplicationRegistrations
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddTransient<ITripsService, TripsService>();
        services.AddTransient<ITripsImageService, TripsImageService>();
        services.AddTransient<IMinioService, MinioService>();
        services.AddTransient<IUploadTripImageUseCase, UploadTripImageUseCase>();
        return services;
    }
}
