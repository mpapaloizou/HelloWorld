using API.Features.V1.Weather.API.DTOs;
using API.Features.V1.Weather.Constants;
using API.Features.V1.Weather.ExternalServices.DTO;
using API.Features.V1.Weather.Services.DTO;
using System.Runtime.CompilerServices;

namespace API.Features.V1.Weather;

/// <summary>
/// Generated - needs review / double check    -    run out of time
/// 
/// Also make the code consistent, e.g. I prefer having curly brackets after if statements instead of one liners and also i prefer using 'is false' instead of '!'
/// </summary>
public static class WeatherMapping
{
    public static ExternalWeatherRequest ToExternalWeatherRequest(this WeatherRequest request)
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (request.Latitude is null || request.Longitude is null)
            throw new ArgumentException("Latitude and Longitude are required.", nameof(request));

        return new ExternalWeatherRequest
        {
            Latitude = request.Latitude.Value,
            Longitude = request.Longitude.Value
        };
    }

    public static WeatherResponse ToWeatherResponse(this ExternalWeatherResponse source)
    {
        if (source is null) throw new ArgumentNullException(nameof(source));
        if (source.main is null) throw new ArgumentException("External response is missing 'main'.", nameof(source));

        // Temperature in °C (because &units=metric)
        var tempC = (int)Math.Round(source.main.temp);

        // Wind in km/h (metric: m/s → km/h)
        var windSpeedMs = source.wind?.speed ?? 0f;
        var windSpeedKmh = (int)Math.Round(windSpeedMs * 3.6f);

        var condition = GetWeatherCondition(source);
        var recommendation = GetRecommendationPhrase(source, condition, tempC);

        return new WeatherResponse
        {
            CurrentTemperatureCelsius = tempC,
            CurrentWindSpeedInKMPerHour = windSpeedKmh,
            // Ties directly to WeatherConditionEnum
            WeatherDescription = condition.ToString(),
            RecommendationPhrase = recommendation
        };
    }

    private static WeatherConditionEnum GetWeatherCondition(ExternalWeatherResponse source)
    {
        var first = source.weather?.FirstOrDefault();
        var main = first?.main;

        if (string.IsNullOrWhiteSpace(main))
            return WeatherConditionEnum.Error;

        // Normalize once
        var mainNorm = main.Trim().ToLowerInvariant();

        // Treat Snow as Snowing
        if (mainNorm == "snow")
            return WeatherConditionEnum.Snowing;

        // Treat Rain / Drizzle / Thunderstorm as Rainy
        if (mainNorm == "rain" || mainNorm == "drizzle" || mainNorm == "thunderstorm")
            return WeatherConditionEnum.Rainy;

        // Clear → Sunny
        if (mainNorm == "clear")
            return WeatherConditionEnum.Sunny;

        // Everything else → Windy (Clouds, Mist, Fog, etc.)
        return WeatherConditionEnum.Windy;
    }

    private static string GetRecommendationPhrase(
        ExternalWeatherResponse source,
        WeatherConditionEnum condition,
        int tempC)
    {
        var first = source.weather?.FirstOrDefault();
        var main = first?.main ?? string.Empty;
        var mainNorm = main.Trim().ToLowerInvariant();

        // Basic flags
        var isRaining = mainNorm is "rain" or "drizzle" or "thunderstorm" || source.rain is not null;
        var isSnowing = mainNorm == "snow";

        // 1. If it's less than 15 degrees Celcius and either raining or snowing
        if (tempC < 15 && (isRaining || isSnowing))
        {
            return RecommendationPhraseConstantStrings.RainyRecommendation;
        }

        // 2. If it's raining
        if (isRaining)
        {
            return RecommendationPhraseConstantStrings.SnowingRecommendation;
        }

        // 3. If it's over 25 degrees Celcius
        if (tempC > 25)
        {
            return RecommendationPhraseConstantStrings.WindyRecommendation;
        }

        // 4. On a sunny day
        if (condition == WeatherConditionEnum.Sunny)
        {
            return RecommendationPhraseConstantStrings.SunnyRecommendation;
        }

        // Fallback -
        // TODO: This breaks the "Return early" design pattern  /  return errors early and the success scenario at the end
        // Ideally I am trying to be consistent and return errors early, and the success scenario at the end
        return RecommendationPhraseConstantStrings.ErrorRecommendation;
    }
}
