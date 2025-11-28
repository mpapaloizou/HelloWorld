using API.Features.V1.Weather.ExternalServices.DTO;
using API.Features.V1.Weather.Services.DTO;
using Bogus;
using ErrorOr;
using WeatherDto = API.Features.V1.Weather.Services.DTO.Weather;

namespace API.Features.V1.Weather.ExternalServices.Services;


/// <summary>
/// Chat GPT generated - needs improvements, run out of time
/// </summary>
public class MockExternalWeatherService : IExternalWeatherService
{
    public Task<ErrorOr<ExternalWeatherResponse>> GetCurrentWeatherAsync(ExternalWeatherRequest request)
    {
        var faker = new Faker();

        // Pick a random high-level condition
        var mainCondition = faker.PickRandom(new[] { "Clear", "Rain", "Snow", "Clouds" });
        var description = mainCondition switch
        {
            "Clear" => "clear sky",
            "Rain" => "light rain",
            "Snow" => "light snow",
            "Clouds" => "scattered clouds",
            _ => "variable conditions"
        };

        // Temperature in °C (like OpenWeather with &units=metric)
        var temp = faker.Random.Float(-5f, 40f);
        var feelsLike = temp + faker.Random.Float(-3f, 3f);

        var response = new ExternalWeatherResponse
        {
            coord = new Coord
            {
                lat = (float)request.Latitude,
                lon = (float)request.Longitude
            },
            weather = new[]
            {
                // use alias to avoid namespace clash
                new WeatherDto
                {
                    id = faker.Random.Int(200, 804),
                    main = mainCondition,
                    description = description,
                    icon = "01d"
                }
            },
            _base = "stations",
            main = new Main
            {
                temp = temp,
                feels_like = feelsLike,
                temp_min = temp - faker.Random.Float(0f, 3f),
                temp_max = temp + faker.Random.Float(0f, 3f),
                pressure = faker.Random.Int(980, 1035),
                humidity = faker.Random.Int(20, 100),
                sea_level = faker.Random.Int(980, 1035),
                grnd_level = faker.Random.Int(980, 1035)
            },
            visibility = faker.Random.Int(1000, 10000),
            wind = new Wind
            {
                speed = faker.Random.Float(0f, 15f), // m/s
                deg = faker.Random.Int(0, 360),
                gust = faker.Random.Float(0f, 25f)
            },
            rain = mainCondition == "Rain"
                ? new Rain { _1h = faker.Random.Float(0.1f, 5f) }
                : null,
            clouds = new Clouds
            {
                all = mainCondition == "Clear"
                    ? faker.Random.Int(0, 10)
                    : faker.Random.Int(20, 100)
            },

            //  ToUnixTimeSeconds() returns long → cast to int
            dt = (int)faker.Date.RecentOffset(1).ToUnixTimeSeconds(),
            sys = new Sys
            {
                type = 1,
                id = faker.Random.Int(1, 9999),
                country = "CY",
                sunrise = (int)faker.Date.SoonOffset(1).ToUnixTimeSeconds(),
                sunset = (int)faker.Date.SoonOffset(1).ToUnixTimeSeconds()
            },
            timezone = 7200, // UTC+2
            id = faker.Random.Int(1, int.MaxValue),
            name = "Mock City",
            cod = 200
        };

        return Task.FromResult<ErrorOr<ExternalWeatherResponse>>(response);
    }
}
