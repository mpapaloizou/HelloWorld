namespace API.Features.V1.Weather.Constants;

public static class RecommendationPhraseConstantStrings
{

    public const string ErrorRecommendation = "Something went wrong.";

    /// <summary>
    /// On a sunny day
    /// </summary>
    public const string SunnyRecommendation = "Don't forget to bring a hat.";
    /// <summary>
    /// If it's over 25 degrees Celcius
    /// </summary>
    public const string WindyRecommendation = "It's a great day for a swim.";
    /// <summary>
    /// If it's less than 15 degrees Celcius and either raining or snowing
    /// </summary>
    public const string RainyRecommendation = "Don't forget to bring a coat.";
    /// <summary>
    /// If it's raining
    /// </summary>
    public const string SnowingRecommendation = "Don't forget the umbrella";
}


