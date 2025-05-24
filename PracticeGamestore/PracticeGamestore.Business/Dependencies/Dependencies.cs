using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.DataAccess;
using PracticeGamestore.DataAccess.Repositories;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.UnitOfWork;

namespace PracticeGamestore.Business.Dependencies;

public static class Dependencies
{
    private static void AddDataAccessServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IGameRepository, GameRepository>();
        services.AddScoped<IPublisherRepository, PublisherRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<IPlatformService, PlatformService>();
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
        services.AddScoped<IGameService, GameService>();
        services.AddScoped<IPublisherService, PublisherService>();
        services.AddScoped<IGenreService, GenreService>();
        services.AddScoped<IPlatformService, PlatformService>();
    }
}