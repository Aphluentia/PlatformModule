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

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ModulesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public OutputDto GetModulesSessionInformation([FromQuery]GetModulesSessionInformationInputDto input) {
            if (input.SessionId== null)
            {
                return OutputHelper.GetOutputMessage(null);
            }
            var sessionData = SessionProvider.GetSessionData((Guid)input.SessionId);
            if (sessionData == null)
                return OutputHelper.GetOutputMessage(null);


            return OutputHelper.GetOutputMessage(
                 new GetModulesSessionInformationOutputDto()
                 {
                     SessionData = sessionData,
                     messageServerPort = 9005,
                     qrCodeData = $"http://localhost:8008/pair?webPlatformId={sessionData.WebPlatformId}"
                 }

                );
                
               
        }

        [HttpGet("fetchModuleData")]
        public OutputDto FetchModuleData([FromQuery] GetModulesSessionInformationInputDto input)
        {
            if (input.SessionId == null)
            {
                return OutputHelper.GetOutputMessage(null);
            }
            var sessionData = SessionProvider.GetSessionData((Guid)input.SessionId);
            if (sessionData == null)
                return OutputHelper.GetOutputMessage(null);


            return OutputHelper.GetOutputMessage(
                 new GetModulesSessionInformationOutputDto()
                 {
                     SessionData = sessionData,
                     messageServerPort = 9005,
                     qrCodeData = $"http://localhost:8008/pair?webPlatformId={sessionData.WebPlatformId}"
                 }

                );


        }


        private static readonly HttpClient client = new HttpClient();

        public static async Task Main()
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync("https://api.example.com/endpoint");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        }




    }
}
