using WebPlatform.Models.Enum;

namespace WebPlatform.Models
{
    public class KafkaMessage
    {
        public string WebPlatformId { get; set; }
        public AppType AppType { get; set; }
        public string Action { get; set; }
        public double Timestamp;
        public string Message { get; set; }
    }
}
