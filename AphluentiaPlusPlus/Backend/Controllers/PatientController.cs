using Backend.Models.Dtos;
using Backend.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemGatewayAPI.Dtos.Entities;
using SystemGatewayAPI.Dtos.Entities.Database;
using SystemGatewayAPI.Dtos.Entities.Secure;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IGatewayProvider _gateway;
        public PatientController(IGatewayProvider provider)
        {
            _gateway = provider;

        }
        // Missing Patient Fetch All

        [HttpGet("{token}/{therapist}")]
        public async Task<IActionResult> PatientAcceptTherapist(string token, string therapist)
        {
            var response = await _gateway.PatientAcceptTherapist(token, therapist);
            if (response.Success)
                return Ok(new OutputDto<string>
                {
                    Message = response.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = response.Message
                    });
        }

        [HttpPost("{token}/Modules")]
        public async Task<IActionResult> PatientAddNewModule(string token, [FromBody] Module module)
        {
            var response = await _gateway.PatientAddNewModule(token, module);
            if (response.Success)
                return Ok(new OutputDto<string>
                {
                    Message = response.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = response.Message
                    });
        } 
        
        [HttpDelete("{token}/Modules/{ModuleId}")]
        public async Task<IActionResult> PatientDeleteModule(string token, Guid ModuleId)
        {
            var response = await _gateway.PatientDeleteModule(token, ModuleId);
            if (response.Success)
                return Ok(new OutputDto<string>
                {
                    Message = response.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = response.Message
                    });
        }

        [HttpGet("{token}")]
        public async Task<IActionResult> FetchPatientInfo(string token)
        {
            var signupResponse = await _gateway.PatientFetchData(token);
            if (signupResponse.Success)
                return Ok(new OutputDto<Patient>
                {
                    Data = signupResponse.Data,
                    Message = "Patient Data Retrieved"
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = signupResponse.Message
                    });
        }

        [HttpGet("{token}/Modules/{ModuleId}")]
        public async Task<IActionResult> PatientFetchModuleById(string token, Guid ModuleId)
        {
            var signupResponse = await _gateway.PatientFetchModuleById(token, ModuleId);
            if (signupResponse.Success)
                return Ok(new OutputDto<Module>
                {
                    Data = signupResponse.Data,
                    Message = "Patient Module Retrieved"
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = signupResponse.Message
                    });
        }

        [HttpGet("{token}/Modules")]
        public async Task<IActionResult> PatientFetchModules(string token)
        {
            var signupResponse = await _gateway.PatientFetchModules(token);
            if (signupResponse.Success)
                return Ok(new OutputDto<ICollection<Module>>
                {
                    Data = signupResponse.Data,
                    Message = "Patient Modules Retrieved"
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = signupResponse.Message
                    });
        }

        [HttpGet("{token}/Therapists")]
        public async Task<IActionResult> PatientsTherapists(string token)
        {
            var response = await _gateway.PatientFetchTherapists(token);
            if (response.Success)
                return Ok(new OutputDto<Associations<SafeTherapist>>
                {
                    Data = response.Data,
                    Message = response.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = response.Message
                    });
        }
        
        [HttpGet("{token}/Modules/{ModuleId}/Status")]
        public async Task<IActionResult> PatientModuleStatusCheck(string token, Guid ModuleId)
        {
            var response = await _gateway.PatientModuleStatusCheck(token, ModuleId);
            if (response.Success)
                return Ok(new OutputDto<ModuleStatusCheck>
                {
                    Data = response.Data,
                    Message = response.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = response.Message
                    });
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> SignupPatient([FromBody] Patient input)
        {
            var signupResponse = await _gateway.PatientRegister(input);
            if (signupResponse.Success)
                return Ok(new OutputDto<string>
                {
                    Message = signupResponse.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = signupResponse.Message
                    });
        }

        [HttpDelete("{token}/{therapist}")]
        public async Task<IActionResult> PatientRejectTherapist(string token, string therapist)
        {
            var response = await _gateway.PatientRejectTherapist(token, therapist);
            if (response.Success)
                return Ok(new OutputDto<string>
                {
                    Message = response.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = response.Message
                    });
        }

        [HttpDelete("{token}")]
        public async Task<IActionResult> PatientRemove(string token)
        {
            var response = await _gateway.PatientRemove(token);
            if (response.Success)
                return Ok(new OutputDto<string>
                {
                    Message = response.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = response.Message
                    });
        }

        [HttpPut("{token}")]
        public async Task<IActionResult> PatientUpdate(string token, [FromBody] Patient patient)
        {
            var response = await _gateway.PatientUpdate(token, patient);
            if (response.Success)
                return Ok(new OutputDto<string>
                {
                    Message = response.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = response.Message
                    });
        }
        
        [HttpPut("{token}/Modules/{ModuleId}/Version/{VersionId}")]
        public async Task<IActionResult> PatientUpdateModuleToVersion(string token, Guid ModuleId, string VersionId)
        {
            var response = await _gateway.PatientUpdateModuleToVersion(token, ModuleId, VersionId);
            if (response.Success)
                return Ok(new OutputDto<string>
                {
                    Message = response.Message
                });
            else
                return BadRequest(
                    new OutputDto<string>
                    {
                        Message = response.Message
                    });
        }

    }
}
