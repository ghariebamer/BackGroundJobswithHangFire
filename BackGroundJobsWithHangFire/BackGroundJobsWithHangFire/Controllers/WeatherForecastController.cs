using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace BackGroundJobsWithHangFire.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            Console.WriteLine(DateTime.Now);
            BackgroundJob.Enqueue(() => WelcomeToUser("gharieb")); /* this method will be execute immediately*/
            Console.WriteLine(DateTime.Now);
            //BackgroundJob.Schedule(() => SendUserEmail("Gharieb@gmail.com"), TimeSpan.FromMinutes(1)); /*will execute this method after 1 minute*/
           
            RecurringJob.AddOrUpdate(() => SendUserEmail("Gharieb@gmail.com"), Cron.Monthly());/* Exceute method every minute*/ 
            
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [ApiExplorerSettings(IgnoreApi =true)]  /* handle Desgin Api */
        public void WelcomeToUser( string Name)
        {
            Console.WriteLine($" welcome to {Name} in MY WebSite");
        }

        [ApiExplorerSettings(IgnoreApi = true)]  /* handle Desgin Api */
        public void SendUserEmail(string Email )
        {
            Console.WriteLine($" Sending Email successfully to this Email:{Email} at {DateTime.Now}");
        }

    }
}