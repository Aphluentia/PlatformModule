using Bridge.Dtos.Entities;
using Newtonsoft.Json;

namespace Bridge.Dtos.Status
{
    public class KafkaStatus
    {
        public static int MessagesReceivedCounter { get; set; } = 0;
        public static int MessagesInQueueCounter { get; set; } = 0;
        public static int MessagesProcessedCounter { get; set; } = 0;
        public static ICollection<Message> MessagesInQueue { get; set; } = new List<Message>();
        public static ICollection<Message> MessagesProcessed { get; set; } = new List<Message>();
        public static Dictionary<string, object> ToJson()
        {
            return new()
        {
            { "MessagesReceivedCounter", MessagesReceivedCounter },
            { "MessagesInQueueCounter", MessagesInQueueCounter },
            { "MessagesProcessedCounter", MessagesProcessedCounter },
            { "MessagesInQueue", JsonConvert.SerializeObject(MessagesInQueue) },
            { "MessagesProcessed", JsonConvert.SerializeObject(MessagesProcessed)},
        };
        }
    }
    
}
