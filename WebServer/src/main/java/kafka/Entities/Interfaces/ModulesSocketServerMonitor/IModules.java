package kafka.Entities.Interfaces.ModulesSocketServerMonitor;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Models.ConnectionRequest;
import org.java_websocket.WebSocket;

public interface IModules {
    void AddNewSocketConnection(ConnectionRequest request, WebSocket conn);

    WebSocket CloseConnection(ConnectionRequest request, WebSocket conn);
}
