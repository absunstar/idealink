using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Test.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
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

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        public async Task<ActionResult> Test()
        {
            var x = new ResponsePaged1();
            x.totalCount = 6;
            x.lstResult = new ResponseTrainingCategory();
            var y = new ResponseTrainingCategory();
            y.Id = "11";
            y.Name = "2222";
            x.lstResult = y;
            //x.lstResult.Add(y);
            return Ok(x);
        }

    }
    public class ResponseTrainingCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class ResponsePaged1 
    {
        public ResponsePaged1()
        {

        }
        public long totalCount;
        public ResponseTrainingCategory lstResult;
    }
}
