using Bridge.Dtos.Enum;

namespace Bridge.Dtos.Dtos
{
    public class RegisterConnectionInputDto
    {
        public string WebPlatformId { get; set; }
        public string ClientSocketAddress { get; set; }
        public ModuleType ModuleType { get; set; }
    }
}
