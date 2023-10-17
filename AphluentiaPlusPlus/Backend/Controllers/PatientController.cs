using Backend.Providers;
using DatabaseApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IGatewayProvider _gateway;
        private readonly ISecurityManagerProvider _security;
        public PatientController(IGatewayProvider provider, ISecurityManagerProvider provider2)
        {
            _gateway = provider;
            _security = provider2;

        }
      
        [HttpGet("{token}")]
        public async Task<ActionResult<Patient>> FetchPatientInfo(string token)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);

            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("Patient is not Valid");

            var details = await _gateway.FetchPatientData(userData.Email, token);
            if (!details.Success) return BadRequest(details.Message);
            return Ok(details);
        }
        
        [HttpPut("{token}")]
        public async Task<IActionResult> UpdatePatientInfo(string token, [FromBody] Patient patient)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);
            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("Therapist is not Valid");

            var details = await _gateway.UpdatePatientData(token, patient);
            if (!details.Success) return BadRequest(details.Message);
            return Ok(details);
        }

        [HttpGet("{token}/{therapist}")]
        public async Task<IActionResult> PatientAcceptTherapist(string token, string therapist)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);
            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("Therapist is not Valid");

            var details = await _gateway.PatientAcceptTherapist(token, therapist);
            if (!details.Success) return BadRequest(details.Message);
            return Ok(details);
        }
        [HttpDelete("{token}/{therapist}")]
        public async Task<IActionResult> PatientRejectTherapist(string token, string therapist)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);
            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("Patient is not Valid");

            var details = await _gateway.PatientRejectTherapist(token, therapist);
            if (!details.Success) return BadRequest(details.Message);
            return Ok(details);
        }

        [HttpGet("{token}/Therapists")]
        public async Task<IActionResult> PatientsTherapists(string token)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);
            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Therapist)
                return BadRequest("Therapist is not Valid");


            var allTherapistPatients = await _gateway.FetchPatientTherapist(token);
            if (!allTherapistPatients.Success) return BadRequest(allTherapistPatients.Message);
            return Ok(allTherapistPatients);
        }
    }
}
