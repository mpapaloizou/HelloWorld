namespace API.Features.V1.Weather.Services.Internal;

/// <summary>
/// Chat gpt generated
/// - Interlocked for thread-safety (First time I see this specific helper class, but i've used semaphore before)
/// - Passing by ref (pointer) so the value is being modified without returning it
/// </summary>
public class WeatherRequestCountService
{
    private int _count = 0;
    public bool ShouldFailThisRequest()
    {
        var current = Interlocked.Increment(ref _count);
        return current % 5 == 0;
    }
}
