using Backend.Helpers;
using Backend.Models.Dtos.Base;
using Backend.Models.Dtos;
using Backend.Models.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Providers;
using Backend.Models.Dtos.Authentication;
using Newtonsoft.Json;
using Backend.Models.Dtos.Session;
using Backend.Models.Entities;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IPublicApiProvider _provider;
        public AuthenticationController(IPublicApiProvider provider) 
        {
            _provider = provider;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<LoginOutputDto>> LoginAndGenerateSession([FromBody] LoginInputDto input)
        {
            var (success, result) =await _provider.Post("/Authentication/GenerateSession", input);
            if (!success)
                return BadRequest();
            var data = JsonConvert.DeserializeObject<OutputMessage<LoginOutputDto>>(result);
            if (!data.Metadata.Success)
                return BadRequest();

            /*
             * var result = await BridgeHelper.CreateConnection(sessionData.WebPlatformId, new Uri(_config.Host));
            var output = new ModulesOutputDto()
            {
                QrCodeData = $"http://localhost:8008/pair?webPlatformId={sessionData.WebPlatformId}",
                Modules = moduleTypes,
            };*/
            return data.Data;


        }
        [HttpPost("Signup")]
        public async Task<ActionResult<RegisterOutputDto>> SignupAndLogin([FromBody] RegisterInputDto input)
        {
            var (success, result) = await _provider.Post("/User/CreateUser", input);
            if (!success)
                return BadRequest();
            var data = JsonConvert.DeserializeObject<OutputMessage<RegisterOutputDto>>(result);
            if (!data.Metadata.Success)
                return BadRequest();
            return data.Data;
        }

        [HttpGet("User")]
        public async Task<ActionResult<RegisterInputDto>> GetUserDetails([FromQuery] SecureSessionDto input)
        {
            var (success, result) = await _provider.Post("/User/RetrieveUserInformation", input);
            if (!success)
                return BadRequest();
            var data = JsonConvert.DeserializeObject<OutputMessage<RegisterInputDto>>(result);
            if (!data.Metadata.Success)
                return BadRequest();
            return data.Data;
        }
    }
}
