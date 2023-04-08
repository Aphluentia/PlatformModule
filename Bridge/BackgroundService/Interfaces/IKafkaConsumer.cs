using Bridge.Dtos.Entities;

namespace Bridge.BackgroundService.Interfaces
{
    public interface IKafkaConsumer
    {
        public bool AddIncomingMessage(Message _message);

    }
}
