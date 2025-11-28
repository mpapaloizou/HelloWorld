using API.Features.V1.Weather.ExternalServices.DTO;
using API.Features.V1.Weather.Services.DTO;
using API.Features.V1.Weather.Services.Internal;
using ErrorOr;
using System.Text.Json;

namespace API.Features.V1.Weather.ExternalServices.Services;

/// <summary>
/// Implementation of IExternalWeatherService for OpenWeatherMap API
/// 
/// Can have another implementation (of a different provider) in the future, implementing the same interface
/// </summary>
public class OpenWeatherService : IExternalWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherRequestCountService _requestCount;

    private const string ApiKey = "20b05f48307adb467d0272e43b15f7f6";

    public OpenWeatherService(HttpClient httpClient, WeatherRequestCountService requestCount)
    {
        _httpClient = httpClient;
        _requestCount = requestCount;
    }

    public async Task<ErrorOr<ExternalWeatherResponse>> GetCurrentWeatherAsync(ExternalWeatherRequest request)
    {
        // Simulate failure every 5th call
        if (_requestCount.ShouldFailThisRequest())
        {
            return Error.Failure(
                code: "WEATHER_RATE_LIMIT",
                description: "Simulated failure: every 5th weather request fails."
            );
        }

        var url =
                  $"https://api.openweathermap.org/data/2.5/weather" +
                  $"?lat={request.Latitude}" +
                  $"&lon={request.Longitude}" +
                  $"&appid={ApiKey}" +
                  $"&units=metric";

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
        }
        catch (Exception ex)
        {
            return Error.Failure(
                code: "WEATHER_HTTP_ERROR",
                description: $"Error calling OpenWeatherMap: {ex.Message}"
            );
        }

        if (response.IsSuccessStatusCode is false)
        {
            var body = await response.Content.ReadAsStringAsync();
            return Error.Failure(
                code: "WEATHER_EXTERNAL_ERROR",
                description: $"OpenWeatherMap returned {(int)response.StatusCode}: {body}"
            );
        }

        using var stream = await response.Content.ReadAsStreamAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var owm = await JsonSerializer.DeserializeAsync<ExternalWeatherResponse>(stream, options);

        if (owm is null)
        {
            return Error.Failure(
                code: "WEATHER_DESERIALIZATION_ERROR",
                description: "Failed to deserialize OpenWeatherMap response."
            );
        }

        return owm;
    }


}