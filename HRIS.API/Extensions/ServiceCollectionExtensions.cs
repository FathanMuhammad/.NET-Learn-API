
using HRIS.API.Data;
using HRIS.API.Services;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHrisServices(
        this IServiceCollection services,
            IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("HrisDB");
        services.AddDbContext<HrisDbContext>(options =>
        options.UseSqlServer(connectionString));
    

        services.AddSingleton<IEmployeeService, InMemoryEmployeeService>();
        services.AddSingleton(TimeProvider.System);
        services.AddHrisMiddlewares();

        return services;
    }
}
