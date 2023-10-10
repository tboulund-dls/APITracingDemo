using Microsoft.AspNetCore.Mvc;
using OpenTelemetry.Trace;

namespace APITraceDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    /*** START OF IMPORTANT CONFIGURATION ***/
    private readonly Tracer _tracer;
    public WeatherForecastController(Tracer tracer)
    {
        _tracer = tracer;
    }
    /*** END OF IMPORTANT CONFIGURATION ***/

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        /*** HOW TO START TRACING ***/
        using var activity = _tracer.StartActiveSpan("GET");
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}