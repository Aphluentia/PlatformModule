package kafka.Entities;

import kafka.Entities.Enum.AppType;
import kafka.Entities.Models.Message;

public interface ISocketMessageHandler {
    void addMessage(Message message, AppType appType);
}
