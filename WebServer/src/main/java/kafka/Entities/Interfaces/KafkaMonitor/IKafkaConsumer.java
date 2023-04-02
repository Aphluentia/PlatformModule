package kafka.Entities.Interfaces.KafkaMonitor;

import kafka.Entities.Models.Message;

public interface IKafkaConsumer {
    void addMessage(Message message);
}
