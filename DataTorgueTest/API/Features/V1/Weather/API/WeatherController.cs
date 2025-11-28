using API.Features.V1.Weather.API.DTOs;
using API.Features.V1.Weather.ExternalServices.DTO;
using API.Features.V1.Weather.ExternalServices.Services;
using API.Features.V1.Weather.Services.DTO;
using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.V1.Weather.API;

[Route("api/v1/[controller]")]
[ApiController]
public class WeatherController : ControllerBase
{
    private readonly IExternalWeatherService _externalWeatherService;

    public WeatherController(IExternalWeatherService externalWeatherService)
    {
        _externalWeatherService = externalWeatherService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(WeatherResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetWeather([FromQuery] WeatherRequest request)
    {
        if (request is null)
        {
            return BadRequest("Invalid or null request");
        }
        if (request.Latitude is null || request.Longitude is null)
        {
            return BadRequest("Invalid or null request");
        }

        var externalWeatherRequest = request.ToExternalWeatherRequest();

       var errorOrResult = await _externalWeatherService.GetCurrentWeatherAsync(externalWeatherRequest);

        if (errorOrResult.IsError)
        {
          
             return StatusCode(StatusCodes.Status500InternalServerError, errorOrResult.FirstError.Description);
        }

        var apiResponse = errorOrResult.Value.ToWeatherResponse();

        return Ok(apiResponse);
    }
}
