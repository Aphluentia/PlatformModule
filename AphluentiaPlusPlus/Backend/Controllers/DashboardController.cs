using Backend.Helpers;
using Backend.Models.Dtos;
using Backend.Models.Dtos.Base;
using Backend.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using QRCoder;
using ZXing.QrCode.Internal;
using Microsoft.Extensions.Options;
using Backend.Configs;
using Backend.Models.Entities;
using Newtonsoft.Json;
using Backend.Models;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private ISessionProvider _provider;
        private BridgeModuleConfigSection _config;
        public DashboardController(SessionProvider provider, IOptions<BridgeModuleConfigSection> config)
        {
            _provider = provider;
            _config = config.Value;
        }


        [HttpGet("FetchPageData")]
        public async Task<OutputDto> GetDashboardData([FromQuery]GetDashboardDataInputDto input) {
            var validation = _provider.ValidateSession(input.SessionId);
            if (validation.Item1 == false)
                return OutputHelper.GetOutputMessage(new()).AddError(validation.Item2);
           
            var sessionData = _provider.GetSessionData((Guid)input.SessionId);
            var output = new GetDashboardDataOutputDto()
            {
                SessionData = sessionData,
                QrCodeData = $"http://localhost:8008/pair?webPlatformId={sessionData.WebPlatformId}"
            };

            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"{_config.Host}:{_config.Port}"); 
            HttpResponseMessage response = await httpClient.GetAsync("api/ServerSocket/Discovery");
            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                output.Server = JsonConvert.DeserializeObject<List<BridgeDiscovery>>(responseContent);
            }
            else
            {
                return OutputHelper.GetOutputMessage(new()).AddError(ApplicationError.DiscoveryNotSuccessful);
            }

            return OutputHelper.GetOutputMessage(
                output
            );


        }





    }
}
