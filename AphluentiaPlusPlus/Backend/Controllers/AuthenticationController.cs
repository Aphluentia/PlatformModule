using Microsoft.AspNetCore.Mvc;
using Backend.Providers;
using Backend.Models.Dtos.Authentication;
using Newtonsoft.Json;
using Backend.Models.Dtos.Session;
using SystemGateway.Dtos.SecurityManager;
using System;
using SystemGateway.Dtos.Enum;
using DatabaseApi.Models.Entities;
using Backend.Models.Entities;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IGatewayProvider _gateway;
        private readonly ISecurityManagerProvider _security;
        public AuthenticationController(IGatewayProvider provider, ISecurityManagerProvider provider2) 
        {
            _gateway = provider;
            _security = provider2;

        }
        [HttpGet("LoginFormData")]
        public async Task<ActionResult<LoginFormDataDto>> GetLoginPageData()
        {
            var data = new LoginFormDataDto
            {
                UserTypes = Enum.GetNames(typeof(UserType)).ToList()
            };
            return Ok(data);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<LoginOutputDto>> LoginAndGenerateSession([FromBody] LoginInputDto input)
        {
            var result = await _gateway.Authenticate(input);
            if (!result.Success)
                return BadRequest(result.Message);

            var secData = new SecurityDataDto
            {
                Email = input.Email,
                UserType = input.UserType

            };
            var token = await _security.GenerateSession(secData);
            if (string.IsNullOrEmpty(token))
                return BadRequest("Failed to Generate Authentication Token");
            

            /*
             * var result = await BridgeHelper.CreateConnection(sessionData.WebPlatformId, new Uri(_config.Host));
            var output = new ModulesOutputDto()
            {
                QrCodeData = $"http://localhost:8008/pair?webPlatformId={sessionData.WebPlatformId}",
                Modules = moduleTypes,
            };*/

            return Ok(new LoginOutputDto
            {
                Token = token,
                Message = "Login Successfull"
            });


        }
      
        [HttpPost("Signup/Patient")]
        public async Task<ActionResult<RegisterOutputDto>> SignupPatient([FromBody] Patient input)
        {
            var result = await _gateway.RegisterPatient(input);
            if (!result.Success)
            {
                return BadRequest(new RegisterOutputDto {Success = false, Message = result.Message });
            }
            
            return new RegisterOutputDto { Success = true, Message = $"Welcome {input.FirstName} {input.LastName}"};
        }
        [HttpPost("Signup/Therapist")]
        public async Task<ActionResult<RegisterOutputDto>> SignupTherapist([FromBody] Therapist input)
        {
            var result = await _gateway.RegisterTherapist(input);
            if (!result.Success)
            {
                return BadRequest(new RegisterOutputDto { Success = false, Message = result.Message });
            }

            return new RegisterOutputDto { Success = true, Message = $"Welcome {input.FirstName} {input.LastName}" };
        }
        [HttpGet("Validate/{token}")]
        public async Task<ActionResult<ValidateSessionOutputDto>> ValidateUser(string token)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return new ValidateSessionOutputDto { IsValidSession = false };
            var userData = await _security.GetTokenData(token);
            var details = await _gateway.GetUserInformation(userData.Email);
            if (!details.Success) return BadRequest(details.Message);
            if (userData.IsExpired) IsValidSession = false;

            return new ValidateSessionOutputDto { IsValidSession = IsValidSession, UserDetails = (UserDetailsDto) details.Data, SessionDetails = ExpirationData.FromSecurityData(userData) };
        }

    }
}
