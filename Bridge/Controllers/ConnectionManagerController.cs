using Bridge.Dtos.Dtos;
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
        [HttpGet("Status")]
        public ConnectionProviderStatus GetData()
        {
            return _status;
        }
        [HttpGet("SocketConnections")]
        public Dictionary<string, object> GetConnections()
        {
            
            var socketConnections = _provider.socketConnections.Select(s => (s.WebPlatformId, s.ClientSocket.Client.RemoteEndPoint.ToString(), s.ModuleType));
            var withstandingSocketConnections = _provider.withstandingSocketConnections.Select(s => (s.Client.RemoteEndPoint.ToString()));
            return new()
            {
                { "WithstandingSocketConnections", JsonConvert.SerializeObject(withstandingSocketConnections)},
                { "SocketConnections", JsonConvert.SerializeObject(socketConnections)},
            };
        }
        [HttpPost("Register")]
        public RegisterConnectionOutputDto RegisterConnection([FromBody] RegisterConnectionInputDto input)
        {
            return new RegisterConnectionOutputDto
            {
                Success = _provider.ProcessSocketConnection(input.WebPlatformId, input.ClientSocketAddress, input.ModuleType)
            };
           
        }


    }
}
