package kafka.Entities.Interfaces.ModulesSocketServerMonitor;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Models.Message;

public interface IKafkaModulesMessageHandler {
    void AddNewMessage(Message newMessage);
    void AddNewModule(Message newMessage);
}
