package kafka.Entities.Interfaces.SocketHubMonitor;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Models.Message;

public interface ISocketHubKafkaMessageHandler {

    void BroadcastNewModuleConnectionRequest(Message newMessage);

    boolean IsValidTargetConnection(String target);
}
