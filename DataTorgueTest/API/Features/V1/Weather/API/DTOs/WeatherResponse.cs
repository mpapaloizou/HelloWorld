using API.Features.V1.Weather.Constants;

namespace API.Features.V1.Weather.API.DTOs;

public record WeatherResponse
{
    public required int CurrentTemperatureCelsius { get; init; }
    public required int CurrentWindSpeedInKMPerHour { get; init; }
    /// <summary>
    /// See <see cref="WeatherConditionEnum"/>
    /// </summary>
    public required string WeatherDescription { get; init; }
    /// <summary>
    /// See <see cref="RecommendationPhraseConstantStrings"/>
    /// </summary>
    public required string RecommendationPhrase { get; init; }
}
