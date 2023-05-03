using Backend.Configs;
using Backend.Helpers;
using Backend.Models;
using Backend.Models.Dtos;
using Backend.Models.Dtos.Base;
using Backend.Models.Dtos.Session;
using Backend.Models.Entities;
using Backend.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly SessionConfigSection _config;
        private readonly SessionProvider _provider;
        public ServicesController(SessionProvider provider, IOptions<SessionConfigSection> config)
        {
            _config = config.Value;
            _provider = provider;
        }

        [HttpGet("KeepAlive")]
        public OutputDto KeepAlive([FromQuery]InputDto input)
        {
            var validation = _provider.ValidateSession(input.SessionId);
            if (validation.Item1 == false)
                return OutputHelper.GetOutputMessage(null).AddError(validation.Item2);

            var sessionData = _provider.GetSessionData((Guid)input.SessionId);
            var result = _provider.KeepAlive(sessionData);
            
            return result == null? OutputHelper.GetOutputMessage(null).AddError(ApplicationError.KeepAliveFailed) : OutputHelper.GetOutputMessage(new VoidDto());
        }
        [HttpGet("GenerateSession")]
        public OutputDto GenerateSession([FromQuery] GenerateSessionInputDto input)
        {
            var sessionData = new SessionData
            {
                WebPlatformId = input.WebPlatformId,
                SessionId = new Guid("c12c9647-1168-4585-af29-032a9168781b"),//Guid.NewGuid()
                ValidityUtcNow = DateTime.UtcNow.AddMinutes(45),
            };
            _provider.StoreSessionData(sessionData);
            return OutputHelper.GetOutputMessage(sessionData);
        }
        [HttpGet("SessionData")]
        public OutputDto GetSessionData([FromQuery]GetSessionDataInputDto input)
        {
            var validation = _provider.ValidateSession(input.SessionId);
            if (validation.Item1 == false)
                return OutputHelper.GetOutputMessage(null).AddError(validation.Item2);
            var result = _provider.GetSessionData((Guid)input.SessionId);
            return OutputHelper.GetOutputMessage(result);

        }
    }
}
