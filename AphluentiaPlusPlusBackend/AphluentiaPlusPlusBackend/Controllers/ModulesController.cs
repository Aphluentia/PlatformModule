using AphluentiaPlusPlusBackend;
using AphluentiaPlusPlusBackend.Controllers;
using Backend.Models.Dtos;
using Backend.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly ILogger<ModulesController> _logger;
        private readonly SocketServerSection _socketServerSettings;

        public ModulesController(IOptions<SocketServerSection> socketServerSettings,ILogger<ModulesController> logger)
        {
            _logger = logger;
            _socketServerSettings = socketServerSettings.Value;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> ModulesPage()
        {

            return 
                new ModulesOutputDto()
                {

                }
        }
    }
}
