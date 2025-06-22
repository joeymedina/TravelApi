using Microsoft.Extensions.DependencyInjection;
using Travel.Domain.Interfaces;
using Travel.Domain.UseCases;

namespace Travel.Domain.Extensions;

public static class DomainRegistrations
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddTransient<ITripsUseCase, TripsUseCase>();
        return services;
    }
}
