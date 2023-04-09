using Bridge.Dtos.Entities;
using Newtonsoft.Json;

namespace Bridge.Dtos.Status
{
    public class KafkaStatus
    {

        public int MessagesReceivedCounter { get; set; } = 0;
        public int MessagesInQueueCounter { get; set; } = 0;
        public int MessagesProcessedCounter { get; set; } = 0;
        public ICollection<Message> MessagesInQueue { get; set; } = new List<Message>();
        public ICollection<Message> MessagesProcessed { get; set; } = new List<Message>();



    }
    
}
