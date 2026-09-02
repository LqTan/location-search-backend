using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Places.Application.Abstractions;
using Places.Application.Places.Queries.GetPlaceById;
using Places.Application.Places.Queries.SearchPlaces;
using Places.Infrastructure.Persistence;
using Places.Infrastructure.Providers;
using Places.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static IServiceCollection AddPlaces(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<PlacesDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection")
            ));
        services.AddScoped<IPlaceRepository, PlaceRepository>();
        services.AddScoped<SearchPlacesHandler>();
        services.AddScoped<GetPlaceByIdHandler>();
        services.AddHttpClient<IPlaceProvider, HerePlaceProvider>(client =>
        {
            client.BaseAddress = new Uri("https://discover.search.hereapi.com/");            
        });

        services.AddControllers()
            .AddApplicationPart(typeof(DependencyInjection).Assembly);

        return services;
    }
}