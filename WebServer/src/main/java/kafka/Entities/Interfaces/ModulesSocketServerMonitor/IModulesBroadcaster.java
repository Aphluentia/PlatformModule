package kafka.Entities.Interfaces.ModulesSocketServerMonitor;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Models.Message;
import org.java_websocket.WebSocket;

public interface IModulesBroadcaster {
    Message FetchMessage();

    WebSocket FetchConnection(String Source, String Target);

    boolean IsValidBroadcastRequest(Message BroadcastRequest);

    void SignalRejectedMessage(Message broadcastRequest);
}
