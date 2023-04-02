using Backend.Helpers;
using Backend.Models.Dtos;
using Backend.Models.Dtos.Base;
using Backend.Models.Dtos.Session;
using Backend.Models.Entities;
using Backend.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServicesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public OutputDto AddSessionData([FromBody] SessionData input)
        {
            SessionProvider.StoreSessionData(input);
            return OutputHelper.GetOutputMessage(new ());
        }
        [HttpGet]
        public OutputDto GetSessionData([FromQuery]GetSessionDataInputDto input)
        {
            var result = SessionProvider.GetSessionData((Guid)input.SessionId);
            if (result == null) OutputHelper.GetOutputMessage(null);
            return OutputHelper.GetOutputMessage(result);

        }
    }
}
