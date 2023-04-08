using Bridge.Dtos.Entities;

namespace Bridge.BackgroundService.Interfaces
{
    public interface IKafkaMessageHandler
    {
        public Message FetchIncomingMessage();
    }
}
