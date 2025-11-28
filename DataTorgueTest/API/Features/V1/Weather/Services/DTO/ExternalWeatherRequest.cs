namespace API.Features.V1.Weather.ExternalServices.DTO;

public record ExternalWeatherRequest
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}
