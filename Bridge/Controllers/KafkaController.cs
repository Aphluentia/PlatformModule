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
        private KafkaStatus _status;
        public KafkaController(ILogger<KafkaController> logger, KafkaStatus status)
        {
            _logger = logger;
            this._status = status;
        }
        [HttpGet("Status")]
        public KafkaStatus GetData()
        {
            return _status;
        }

      
    }
}