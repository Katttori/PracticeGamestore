using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.DataAccess;
using PracticeGamestore.DataAccess.Repositories;
using PracticeGamestore.DataAccess.UnitOfWork;
namespace PracticeGamestore.Business.Dependencies;

public static class Dependencies
{
    private static void AddDataAccessServices(this IServiceCollection services)
    {
           services.AddScoped<IUnitOfWork, UnitOfWork>();
           services.AddScoped<IPublisherRepository, PublisherRepository>();
    }

    private static void RegisterDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<GamestoreDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("GamestoreDatabase")));
    }

    public static void AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterDbContext(configuration);
        services.AddDataAccessServices();
        services.AddScoped<IPublisherService, PublisherService>();
    }
}