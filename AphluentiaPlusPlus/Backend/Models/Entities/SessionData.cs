namespace Backend.Models.Entities
{
    public class SessionData
    {
        public string WebPlatformId { get; set; }
        public Guid? SessionId { get; set; }

        public DateTime ValidityUtcNow { get; set; }
    }
}
