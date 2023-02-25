package kafka.Entities;

import kafka.Entities.Models.Message;

public interface IKafkaConsumer {
    void addMessage(Message message);
}
