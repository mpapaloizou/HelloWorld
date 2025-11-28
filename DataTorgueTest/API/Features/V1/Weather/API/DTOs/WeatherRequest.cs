namespace API.Features.V1.Weather.API.DTOs;

public record WeatherRequest
{
    public required double? Latitude { get; init; }
    public required double? Longitude { get; init; }
}
