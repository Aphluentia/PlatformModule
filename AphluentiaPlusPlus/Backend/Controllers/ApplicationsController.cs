using Backend.Models.Dtos.Session;
using Backend.Models.Dtos;
using Backend.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SystemGatewayAPI.Dtos.Entities.Database;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationsController : ControllerBase
    {
        private readonly IGatewayProvider _gateway;
        public ApplicationsController(IGatewayProvider provider)
        {
            _gateway = provider;

        }

        [HttpGet]
        public async Task<IActionResult> FetchAllApplications()
        {
            var loginResponse = await _gateway.ApplicationFetchAll();
            if (loginResponse.Success)
                return Ok(new OutputDto<ICollection<Application>>
                {
                    Data = loginResponse.Data,
                    Message = "Applications Fetched"
                });
            else
                return BadRequest(
                    new OutputDto<ICollection<Application>>
                    {
                        Message = loginResponse.Message
                    });
        }

    }
}
