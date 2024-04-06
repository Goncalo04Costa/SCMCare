using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models;
using WebApplication1.Conecta;

namespace WebApplication1.Controllers
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
        private readonly SCMDbContext _context;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, SCMDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<ActionResult<Sobremesa>> Get()
        {
            var sobremesa = await _context.Sobremesas.FirstOrDefaultAsync(m => m.Id == 2);

            if (sobremesa == null)
            {
                return NotFound(); // Retorna um 404 Not Found se a sobremesa não for encontrada
            }

            return Ok(sobremesa); // Retorna a sobremesa encontrada
        }

    }
}
