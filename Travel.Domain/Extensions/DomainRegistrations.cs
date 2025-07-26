using Microsoft.Extensions.DependencyInjection;

namespace Travel.Domain.Extensions;
public static class DomainRegistrations
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services;
    }
}
