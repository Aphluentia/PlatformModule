using Backend.Helpers;
using Backend.Models.Dtos;
using Backend.Models.Dtos.Base;
using Backend.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZXing.Common;
using ZXing;
using ZXing.QrCode;
using System.Drawing;
using System.Drawing.Imaging;
using ZXing.Rendering;

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
            var httpContext = _httpContextAccessor.HttpContext;
            if (input.SessionId== null)
            {
                return OutputHelper.GetOutputMessage(null);
            }
            var sessionData = SessionProvider.GetSessionData((Guid)input.SessionId);
            if (sessionData == null)
                return OutputHelper.GetOutputMessage(null);

            var writer = new BarcodeWriter<Bitmap>
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions { Width = 500, Height = 200 }
            };

            var barcodeBitmap = writer.Write($"http://localhost:8008/pair?WebPlatformId={sessionData.WebPlatformId}", new ConsoleRenderer());
            var base64String = "";
            using (var memoryStream = new MemoryStream())
            {
                barcodeBitmap.Save(memoryStream, ImageFormat.Png);
                var byteArr = memoryStream.ToArray();
                base64String = Convert.ToBase64String(byteArr);
            }

            return OutputHelper.GetOutputMessage(
                 new GetModulesSessionInformationOutputDto()
                 {
                     SessionData = sessionData,
                     QrCodeB64 = base64String
                 }

                );
                
               
        }
       



    }
}
