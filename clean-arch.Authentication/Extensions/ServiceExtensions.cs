// using clean_arch.Infrastructure.Interfaces;
// using clean_arch.Infrastructure.Persistence;
// using clean_arch.Infrastructure.Services;
// using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace clean_arch.Authentication.Extensions;

public static class ServiceExtensions
{
    public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
    {
        // services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("MainDb")));
        // services.AddScoped<IEmployee, EmployeeService>();
        // services.AddControllers();
        // services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();
    }
}