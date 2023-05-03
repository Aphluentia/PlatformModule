using Bridge.Dtos.Enum;

namespace Backend.Models.Entities
{
    public class BridgeDiscovery
    {
        public ModuleType moduleType { get; set; }
        public int serverPort { get; set; }
    }
}
