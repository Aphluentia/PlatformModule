package kafka.Entities.Interfaces.SocketHubMonitor;

import org.java_websocket.WebSocket;

public interface ISocketHub {

    void AddNewSocketConnection(String PlatformId, WebSocket conn);

    void CloseConnection(String platformId, WebSocket conn);
}
