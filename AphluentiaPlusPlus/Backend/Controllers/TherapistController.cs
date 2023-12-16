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
    public class TherapistController : ControllerBase
    {
        private readonly IGatewayProvider _gateway;
        public TherapistController(IGatewayProvider provider)
        {
            _gateway = provider;

        }
        [HttpGet("{token}/{patient}")]
        public async Task<IActionResult> TherapistAcceptPatient(string token, string patient)
        {
            var signupResponse = await _gateway.TherapistAcceptPatient(token, patient);
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

        [HttpDelete("{token}")]
        public async Task<IActionResult> TherapistDelete(string token)
        {
            var signupResponse = await _gateway.TherapistDelete(token);
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

        [HttpGet("{token}")]
        public async Task<IActionResult> FetchTherapistInfo(string token)
        {
            var response = await _gateway.TherapistFetchData(token);
            if (response.Success)
                return Ok(new OutputDto<Therapist>
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
        [HttpGet("{token}/Patients/{PatientEmail}")]
        public async Task<IActionResult> TherapistFetchPatientData(string token, string PatientEmail)
        {
            var response = await _gateway.TherapistFetchPatientData(token, PatientEmail);
            if (response.Success)
                return Ok(new OutputDto<SafePatient>
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

        [HttpGet("{token}/Patients")]
        public async Task<IActionResult> TherapistFetchPatients(string token)
        {
            var response = await _gateway.TherapistFetchPatients(token);
            if (response.Success)
                return Ok(new OutputDto<Associations<SafePatient>>
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
        [HttpGet("{token}/Patients/All")]
        public async Task<IActionResult> TherapistFetchAllPatients(string token)
        {
            var response = await _gateway.TherapistFetchAllPatients(token);
            if (response.Success)
                return Ok(new OutputDto<ICollection<SafePatient>>
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
        [HttpGet("{token}/Patients/{PatientEmail}/Modules/{ModuleId}")]
        public async Task<IActionResult> TherapistFetchPatientsModuleById(string token, string PatientEmail, Guid ModuleId)
        {
            var response = await _gateway.TherapistFetchPatientsModuleById(token, PatientEmail, ModuleId);
            if (response.Success)
                return Ok(new OutputDto<Module>
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
        [HttpGet("{token}/Patients/{PatientEmail}/Modules")]
        public async Task<IActionResult> TherapistFetchPatientsModules(string token, string PatientEmail)
        {
            var response = await _gateway.TherapistFetchPatientsModules(token, PatientEmail);
            if (response.Success)
                return Ok(new OutputDto<ICollection<Module>>
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
        [HttpGet("{token}/Patients/{PatientEmail}/Modules/{ModuleId}/Status")]
        public async Task<IActionResult> TherapistModuleStatusCheck(string token, string PatientEmail, Guid ModuleId)
        {
            var response = await _gateway.TherapistModuleStatusCheck(token, PatientEmail, ModuleId);
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

        [HttpPost("Signup/Therapist")]
        public async Task<IActionResult> SignupTherapist([FromBody] Therapist input)
        {
            var signupResponse = await _gateway.TherapistRegister(input);
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

        [HttpDelete("{token}/{patient}")]
        public async Task<IActionResult> TherapistRejectPatient(string token, string patient)
        {
            var signupResponse = await _gateway.TherapistRejectPatient(token, patient);
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

        [HttpPut("{token}")]
        public async Task<IActionResult> TherapistUpdate(string token, [FromBody] Therapist therapist)
        {
            var signupResponse = await _gateway.TherapistUpdate(token, therapist);
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
        [HttpPut("{token}/Patients/{PatientEmail}/Modules/{ModuleId}")]
        public async Task<IActionResult> TherapistUpdatePatientModule(string token, string PatientEmail, Guid ModuleId, [FromBody] Module module)
        {
            var signupResponse = await _gateway.TherapistUpdatePatientModule(token, PatientEmail, ModuleId, module);
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
        [HttpPut("{token}/Patients/{PatientEmail}/Modules/{ModuleId}/Version/{VersionId}")]
        public async Task<IActionResult> TherapistUpdatePatientModuleToVersion(string token, string PatientEmail, Guid ModuleId, string VersionId)
        {
            var signupResponse = await _gateway.TherapistUpdatePatientModuleToVersion(token, PatientEmail, ModuleId, VersionId);
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


        [HttpPost("{token}/Patients/{PatientEmail}/Modules/{ModuleId}/Profile/{ProfileName}")]
        public async Task<IActionResult> PatientModuleCreateProfile(string token, string PatientEmail, Guid ModuleId, string ProfileName)
        {
            var response = await _gateway.TherapistPatientModuleCreateProfile(token, PatientEmail, ModuleId, ProfileName);
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

        [HttpDelete("{token}/Patients/{PatientEmail}/Modules/{ModuleId}/Profile/{ProfileName}")]
        public async Task<IActionResult> PatientModuleDeleteProfile(string token, string PatientEmail, Guid ModuleId, string ProfileName)
        {
            var response = await _gateway.TherapistPatientModuleDeleteProfile(token, PatientEmail, ModuleId, ProfileName);
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
