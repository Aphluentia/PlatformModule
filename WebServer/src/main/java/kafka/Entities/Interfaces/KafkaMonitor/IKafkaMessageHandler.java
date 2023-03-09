package kafka.Entities.Interfaces.KafkaMonitor;

import kafka.Entities.Models.Message;

public interface IKafkaMessageHandler {
    Message FetchMessage();
}
