using Microsoft.AspNetCore.Mvc;
using Backend.Providers;
using Newtonsoft.Json;
using System;
using SystemGateway.Dtos.Enum;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using SystemGatewayAPI.Dtos.Entities.Database;
using SystemGatewayAPI.Dtos.Entities.SecurityManager;
using Backend.Models.Dtos.Input;
using Backend.Models.Dtos;
using Backend.Models.Dtos.Session;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IGatewayProvider _gateway;
        public AuthenticationController(IGatewayProvider provider) 
        {
            _gateway = provider;

        }
     
        [HttpPost("Login")]
        public async Task<IActionResult> LoginAndGenerateSession([FromBody] LoginInputDto input)
        {
            var loginResponse = await _gateway.AuthenticateAndGenerateToken(input.UserType, input.Email, input.Password);
            if (loginResponse.Success)
                return Ok(new OutputDto<string>
                {
                    Data = loginResponse.Data,
                    Message = "Login Successful"
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = loginResponse.Message
                    });
            /*
             * var result = await BridgeHelper.CreateConnection(sessionData.WebPlatformId, new Uri(_config.Host));
            var output = new ModulesOutputDto()
            {
                QrCodeData = $"http://localhost:8008/pair?webPlatformId={sessionData.WebPlatformId}",
                Modules = moduleTypes,
            };*/
        }
      
        [HttpGet("{token}")]
        public async Task<IActionResult> ValidateUser(string token)
        {
            var loginResponse = await _gateway.AuthenticationFetchUserDetails(token);
            if (loginResponse.Success)
                return Ok(new OutputDto<SessionData>
                {
                    Data = loginResponse.Data,
                    Message = "Details Fetched"
                });
            else
                return BadRequest(
                    new OutputDto<SessionData>
                    {
                        Message = loginResponse.Message
                    });
        }

    }
}
