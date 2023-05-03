namespace Backend.Models.Dtos.Base
{
    public class Metadata
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public bool Success { get; set; }
        public IList<string> Errors { get; set; } = new List<string>();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
