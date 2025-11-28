using API.Features.V1.Weather.ExternalServices.Services;
using API.Features.V1.Weather.Services.Internal;

namespace API.Features.V1;

public static class DIServices
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<WeatherRequestCountService>();

        services.AddHttpClient<IExternalWeatherService, OpenWeatherService>();
        //services.AddScoped<IExternalWeatherService, OpenWeatherService>();

        return services;
    }
}