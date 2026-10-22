
using HRIS.API.Services;

namespace HRIS.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHrisServices(
        this IServiceCollection services)
    {
        services.AddSingleton<IEmployeeService, InMemoryEmployeeService>();
        services.AddSingleton(TimeProvider.System);

        return services;
    }
}
