using Backend.Configs;
using Backend.Helpers;
using Backend.Models.Dtos.Authentication;
using Backend.Models.Dtos.Base;
using Backend.Models.Dtos.Modules;
using Backend.Models.Entities;
using Backend.Models.Enums;
using Backend.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModulesController : ControllerBase
    {
        private readonly IPublicApiProvider _provider;
        private readonly BridgeModuleConfigSection _config;
        public ModulesController(IPublicApiProvider provider, IOptions<BridgeModuleConfigSection> options)
        {
            _provider = provider;
            _config = options.Value;
        }
        [HttpGet]
        public async Task<ActionResult<GetModulesPageOutputDto>> LoginAndGenerateSession([FromQuery] GetModulesPageInputDto input)
        {
            var result = await BridgeHelper.CreateConnection(input.WebPlatformId.ToString(), new Uri(_config.ConnectionString));
            if (!result)
                return NotFound();
            var modules = ((ModuleType[])Enum.GetValues(typeof(ModuleType))).Where(moduleType => moduleType != ModuleType.AphluentiaPlusPlus_Web)
                    .Select(moduleType => new ModuleTypeDetails { Code = moduleType })
                    .ToList();
            return new GetModulesPageOutputDto()
            {
                QrCodeUrl = $"http://localhost:8008/pair?webPlatformId={input.WebPlatformId}",
                Modules = modules,
            };


        }
    }
}
