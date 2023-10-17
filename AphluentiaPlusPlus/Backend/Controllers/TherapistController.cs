using Backend.Models.Dtos.Authentication;
using Backend.Models.Entities;
using Backend.Providers;
using DatabaseApi.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TherapistController : ControllerBase
    {
        private readonly IGatewayProvider _gateway;
        private readonly ISecurityManagerProvider _security;
        public TherapistController(IGatewayProvider provider, ISecurityManagerProvider provider2)
        {
            _gateway = provider;
            _security = provider2;

        }
        [HttpGet("{token}")]
        public async Task<ActionResult<ICollection<Therapist>>> FetchTherapistInfo(string token)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
         
            var details = await _gateway.FetchAllTherapists(token);
            if (!details.Success) return BadRequest(details.Message);
            return Ok(details);
        }

        [HttpGet("{token}/Email/{email}")]
        public async Task<ActionResult<Therapist>> FetchTherapistInfo(string token, string email)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);

            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("Therapist is not Valid");
            if (email != userData.Email)
                return Unauthorized("Cannot get Data from Another Therapist");
            var details = await _gateway.GetTherapistData(email, token);
            if (!details.Success) return BadRequest(details.Message);
            return Ok(details);
        }

        [HttpPut("{token}")]
        public async Task<IActionResult> UpdateTherapistInfo(string token, [FromBody] Therapist therapist)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);
            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("Therapist is not Valid");

            var details = await _gateway.UpdateTherapistData(token, therapist);
            if (!details.Success) return BadRequest(details.Message);
            return Ok(details);
        }

        [HttpGet("{token}/{patient}")]
        public async Task<IActionResult> TherapistAcceptPatient(string token, string patient)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);
            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("Therapist is not Valid");

            var details = await _gateway.TherapistAcceptPatient(token, patient);
            if (!details.Success) return BadRequest(details.Message);
            return Ok(details);
        }
        [HttpDelete("{token}/{patient}")]
        public async Task<IActionResult> TherapistRejectPatient(string token, string patient)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);
            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("Therapist is not Valid");

            var details = await _gateway.TherapistRejectPatient(token, patient);
            if (!details.Success) return BadRequest(details.Message);
            return Ok(details);
        }

        [HttpGet("{token}/Patients")]
        public async Task<IActionResult> TherapistPatients(string token)
        {
            var IsValidSession = await _security.ValidateSession(token);
            if (!IsValidSession) return Unauthorized("Session is not Valid");
            var userData = await _security.GetTokenData(token);
            if (userData == null || userData.UserType == SystemGateway.Dtos.Enum.UserType.Patient)
                return BadRequest("Therapist is not Valid");


            var allTherapistPatients = await _gateway.FetchTherapistPatients(token);
            if (!allTherapistPatients.Success) return BadRequest(allTherapistPatients.Message);
            return Ok(allTherapistPatients);
        }


      
       
    }
}
