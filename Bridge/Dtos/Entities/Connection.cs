using Bridge.Dtos.Enum;
using Confluent.Kafka;
using static System.Collections.Specialized.BitVector32;

namespace Bridge.Dtos.Entities
{
    public class Connection
    {
        public ModuleType ModuleType { get; set; }
        public string? WebPlatformId { get; set; }
        public string? ModuleId { get; set; }
        public string ToJson()
        {
            return $"{{ ModuleType: {ModuleType.GetName(this.ModuleType)}, " +
                $"\nWebPlatformId: {this.WebPlatformId}," +
                $"\nModuleId: {this.ModuleId} }}";
        }
        public static ICollection<string> ToJson(ICollection<Connection> connections)
        {
            return connections.Select(s => s.ToJson()).ToList();
        }
    }
}
