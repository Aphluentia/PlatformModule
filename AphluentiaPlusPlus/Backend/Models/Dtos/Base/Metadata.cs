namespace Backend.Models.Dtos.Base
{
    public class Metadata
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public bool Success { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
