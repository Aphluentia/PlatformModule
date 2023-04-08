using Bridge.BackgroundService.Monitors;
using Bridge.Dtos.Status;
using Microsoft.AspNetCore.Mvc;

namespace Bridge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KafkaController : ControllerBase
    {
       

        private readonly ILogger<KafkaController> _logger;
        public KafkaController(ILogger<KafkaController> logger)
        {
            _logger = logger;
        }
        [HttpGet("Status")]
        public Dictionary<string, object> GetData()
        {
            return KafkaStatus.ToJson();
        }

      
    }
}