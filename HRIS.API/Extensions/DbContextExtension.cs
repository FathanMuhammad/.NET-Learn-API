using HRIS.API.Data;
using Microsoft.EntityFrameworkCore;

namespace HRIS.API.Extensions;

public static class DbContextExtension
{
    public static IServiceCollection AddDbContext(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //var connectionString = configuration.GetConnectionString("HrisDB");
        //services.AddDbContext<HrisDbContext>(options =>
        //options.UseSqlServer(connectionString));

        return services;

    }
}
