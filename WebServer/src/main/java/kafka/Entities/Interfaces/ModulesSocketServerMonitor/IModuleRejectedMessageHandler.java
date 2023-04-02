package kafka.Entities.Interfaces.ModulesSocketServerMonitor;

import kafka.Entities.Models.Message;

public interface IModuleRejectedMessageHandler {
    void InsertRejectedMessage(Message message);

    Message FetchRejectedMessage();
}
