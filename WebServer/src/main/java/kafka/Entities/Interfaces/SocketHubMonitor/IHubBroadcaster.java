package kafka.Entities.Interfaces.SocketHubMonitor;

import kafka.Entities.Models.Message;
import org.java_websocket.WebSocket;

public interface IHubBroadcaster {
    Message FetchForBroadcastRequest();

    WebSocket FetchConnection(String Target);
}
