namespace Backend.Models.Entities
{
    public class SessionData
    {
        public string WebPlatformId { get; set; }
        public Guid? SessionId { get; set; }

        public bool isValidSession { get; set; }
    }
}
