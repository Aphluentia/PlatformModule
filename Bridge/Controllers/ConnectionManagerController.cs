using Bridge.Dtos.Dtos;
using Bridge.Dtos.Entities;
using Bridge.Dtos.Status;
using Bridge.Providers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bridge.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ConnectionManagerController : ControllerBase
    {


        private readonly ILogger<ConnectionManagerController> _logger;
        private readonly ConnectionManagerProvider _provider;
        private ConnectionProviderStatus _status;
        public ConnectionManagerController(ILogger<ConnectionManagerController> logger, ConnectionManagerProvider provider, ConnectionProviderStatus status)
        {

            _logger = logger;
            _provider = provider;
            _status = status;
        }
        [HttpGet("Connection")]
        public ICollection<Connection> GetConnections()
        {
            return _provider.connections;
        }
        [HttpPost("Connection")]
        public RegisterConnectionOutputDto RegisterWebPlatformConnection([FromBody] RegisterConnectionInputDto register)
        {
            return new RegisterConnectionOutputDto { Success = _provider.RegisterWebPlatformConnection(register.WebPlatformId) };
        }
        [HttpDelete("Connection")]
        public RegisterConnectionOutputDto RemoveWebPlatformConnection([FromBody] RegisterConnectionInputDto register)
        {
            return new RegisterConnectionOutputDto { Success = _provider.RemoveWebPlatformConnection(register.WebPlatformId) };
        }
        [HttpGet("Poll")]
        public PollMessagesOutputDto PollMessages([FromQuery] PollMessagesInputDto connection)
        {
            return new PollMessagesOutputDto { Success=true, Messages = _provider.PollMessages(connection.WebPlatformId, connection.ModuleType) };
        }




    }
}
