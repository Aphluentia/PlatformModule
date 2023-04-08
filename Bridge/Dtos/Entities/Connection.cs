using Bridge.Dtos.Enum;

namespace Bridge.Dtos.Entities
{
    public class Connection
    {
        public ModuleType ModuleType { get; set; }
        public string WebPlatformId { get; set; }
        public string ModuleId { get; set; }

    }
}
