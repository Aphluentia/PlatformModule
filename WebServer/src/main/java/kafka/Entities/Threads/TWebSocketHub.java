package kafka.Entities.Threads;

import com.google.gson.Gson;
import kafka.Entities.Enum.AppType;
import kafka.Entities.ISocketConnectionHandler;
import kafka.Entities.Models.ConnectionRequest;
import org.java_websocket.WebSocket;
import org.java_websocket.handshake.ClientHandshake;
import org.java_websocket.server.WebSocketServer;

import java.net.InetSocketAddress;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Set;

public class TWebSocketHub extends Thread {
    private int port;
    private boolean stopFlag;

    /**
     * Initialize TServer
     *
     * @param _port    Server Port
     */
    public TWebSocketHub(HashMap<AppType, Integer> _typePorts, int _port) {
        this.stopFlag = false;
        new WebSocketHub(_port, _typePorts).start();


    }

    /**
     * <p>stopFlag flag set to true, process ends</p>
     */
    public void stopProcess() {
        this.stopFlag = true;

    }


    private class WebSocketHub extends WebSocketServer {
        private final HashMap<AppType, Integer> typePorts;
        private HashMap<AppType, Set<WebSocket>> conns;
        public WebSocketHub(int PORT, HashMap<AppType, Integer> _typePorts) {
            super(new InetSocketAddress(PORT));
            this.conns = new HashMap<>();
            for (AppType aT: AppType.values()){
                this.conns.put(aT, new HashSet<WebSocket>());
            }
            this.typePorts = _typePorts;
        }

        @Override
        public void onOpen(WebSocket conn, ClientHandshake handshake) {
            System.out.println("New connection to hub from " + conn.getRemoteSocketAddress().getAddress().getHostAddress());
        }

        @Override
        public void onClose(WebSocket conn, int code, String reason, boolean remote) {
            System.out.println("Closed connection to " + conn.getRemoteSocketAddress().getAddress().getHostAddress());
        }

        @Override
        public void onMessage(WebSocket conn, String message) {
            System.out.println("Message from client: " + message);
            ConnectionRequest conReq = new Gson().fromJson(message, ConnectionRequest.class);
            AppType conType = null;
            System.out.println(conReq);
            switch(conReq.getAction()){
                case NEW_CONNECTION:
                    conns.get(conReq.getAppType()).add(conn);
                    conn.send("CONNECT:"+this.typePorts.get(conReq.getAppType()).toString());
                    break;
                case CLOSE_CONNECTION:
                    conns.get(conReq.getAppType()).remove(conn);
                    conn.send("CLOSED");
                    break;
                default:
                    conn.send("NOT_AVAILABLE");
            }
        }

        @Override
        public void onError(WebSocket conn, Exception ex) {

        }

        @Override
        public void onStart() {

        }
    }
}