using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PracticeGamestore.Business.Services.Blacklist;
using PracticeGamestore.Business.Services.Country;
using PracticeGamestore.Business.Services.File;
using PracticeGamestore.Business.Services.Game;
using PracticeGamestore.DataAccess;
using PracticeGamestore.Business.Services.Publisher;
using PracticeGamestore.Business.Services.Platform;
using PracticeGamestore.Business.Services.Genre;
using PracticeGamestore.Business.Services.HeaderHandle;
using PracticeGamestore.Business.Services.Location;
using PracticeGamestore.Business.Services.Order;
using PracticeGamestore.Business.Services.User;
using PracticeGamestore.DataAccess.Repositories.Blacklist;
using PracticeGamestore.DataAccess.Repositories.Country;
using PracticeGamestore.DataAccess.Repositories.File;
using PracticeGamestore.DataAccess.Repositories.Game;
using PracticeGamestore.DataAccess.Repositories.Genre;
using PracticeGamestore.DataAccess.Repositories.Platform;
using PracticeGamestore.DataAccess.Repositories.Publisher;
using PracticeGamestore.DataAccess.Repositories.Order;
using PracticeGamestore.DataAccess.Repositories.User;
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
        services.AddScoped<IPlatformRepository, PlatformRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<IBlacklistRepository, BlacklistRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
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
        services.AddScoped<ICountryService, CountryService>();
        services.AddScoped<IBlacklistService, BlacklistService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILocationService, LocationService>();
        services.AddScoped<IHeaderHandleService, HeaderHandleService>();
    }
}