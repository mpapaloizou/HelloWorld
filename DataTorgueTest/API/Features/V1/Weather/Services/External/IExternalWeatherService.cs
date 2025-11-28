using API.Features.V1.Weather.ExternalServices.DTO;
using API.Features.V1.Weather.Services.DTO;
using ErrorOr;

namespace API.Features.V1.Weather.ExternalServices.Services;

public interface IExternalWeatherService
{
    public Task<ErrorOr<ExternalWeatherResponse>> GetCurrentWeatherAsync(ExternalWeatherRequest request);
}
