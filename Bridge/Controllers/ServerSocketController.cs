using Bridge.Dtos.Entities;
using Bridge.Dtos.Enum;
using Bridge.Dtos.Status;
using Bridge.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Bridge.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerSocketController : ControllerBase
    {
        private readonly ILogger<ServerSocketController> _logger;
        private readonly SocketServerProvider _provider;
        private ServerSocketStatus _status;
        public ServerSocketController(ILogger<ServerSocketController> logger, SocketServerProvider provider, ServerSocketStatus status)
        {
            _logger = logger;
            _provider = provider;
            _status = status;
        }
        [HttpGet("Connections")]
        public ServerSocketStatus GetConnections()
        {
            return _status;

        }
        [HttpGet("Servers")]
        public ICollection<SocketServer> Discovery()
        {
            return _provider._servers;

        }
    }
}
