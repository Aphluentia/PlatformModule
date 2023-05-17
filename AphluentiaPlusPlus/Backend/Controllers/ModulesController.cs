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
using Backend.Models.Enums;
using System.Text;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private ISessionProvider _provider;
        public BridgeModuleConfigSection _config;
        public ModulesController(SessionProvider provider, IOptions<BridgeModuleConfigSection> config)
        {
            _provider = provider;
            _config = config.Value;
        }


        [HttpGet("Setup")]
        public async Task<OutputDto> GetDashboardData([FromQuery]ModulesInputDto input) {
            var validation = _provider.ValidateSession(input.SessionId);
            if (validation.Item1 == false)
                return OutputHelper.GetOutputMessage(new()).AddError(validation.Item2);
           
            var sessionData = _provider.GetSessionData((Guid)input.SessionId);
            var moduleTypes = Enum.GetValues(typeof(ModuleType))
                    .Cast<ModuleType>()
                    .Where(mType => mType != ModuleType.AphluentiaPlusPlus_Web)
                    .Select(ModuleTypeDto.FromModuleType)
                    .ToList();

            var result = await BridgeHelper.CreateConnection(sessionData.WebPlatformId, new Uri(_config.Host));
            var output = new ModulesOutputDto()
            {
                QrCodeData = $"http://localhost:8008/pair?webPlatformId={sessionData.WebPlatformId}",
                Modules = moduleTypes,
            };
            return OutputHelper.GetOutputMessage(
                output
            );


        }





    }
}
