using Bridge.Dtos.Enum;

namespace Bridge.Dtos.Entities
{
    public class Message
    {
        public Enum.Action Action { get; set; }
        public string SourceId { get; set; }
        public string TargetId { get; set; }
        public ModuleType SourceModuleType { get; set; }
        public ModuleType TargetModuleType { get; set; }
        public string? Section { get; set; }
        public string Timestamp { get; set; }

        public string ToJson()
        {
            return $"{{Action: {Enum.Action.GetName(this.Action)}, " +
                $"\nSourceId: {SourceId}, " +
                $"\nTargetId: {TargetId}, " +
                $"\nSourceModuleType: {ModuleType.GetName(this.SourceModuleType)}, " +
                $"\nTargetModuleType: {ModuleType.GetName(this.TargetModuleType)}, " +
                $"\nSection: {Section}, " +
                $"\nTimestamp: {Timestamp}}}";
        }
        public static ICollection<string> ToJson(ICollection<Message> messages)
        {
            return messages.Select(s => s.ToJson()).ToList();
        }
    }
}
