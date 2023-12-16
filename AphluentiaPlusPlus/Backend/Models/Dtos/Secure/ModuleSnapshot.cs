using SystemGatewayAPI.Dtos.Entities.Database;

namespace Backend.Models.Dtos.Secure
{
    public class ModuleSnapshot
    {
        public Guid ModuleId { get; set; }
        public string Checksum { get; set; }
        public DateTime Timestamp { get; set; }
        public static ModuleSnapshot FromModule(Module module)
        {
            return new ModuleSnapshot
            {
                ModuleId = module.Id,
                Checksum = module.ModuleData.Checksum,
                Timestamp = (DateTime)(module.ModuleData.Timestamp == null ? DateTime.UtcNow : module.ModuleData.Timestamp)
            };
        }
    }
}
