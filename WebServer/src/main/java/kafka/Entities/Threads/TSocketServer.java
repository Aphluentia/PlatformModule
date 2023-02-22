package kafka.Entities.Threads;

import kafka.Entities.Enum.AppType;
import kafka.Entities.ISocketConnectionHandler;
import kafka.Entities.Models.Message;
import org.java_websocket.WebSocket;
import org.java_websocket.handshake.ClientHandshake;
import org.java_websocket.server.WebSocketServer;

import java.net.InetSocketAddress;
import java.util.HashSet;
import java.util.Set;

public class TSocketServer extends Thread {
    private final ISocketConnectionHandler mWebSockets;
    private final AppType appType;
    private int port;
    private boolean stopFlag;
    private SocketServer server;

    /**
     * Initialize TServer
     *
     * @param _mserver MServer Monitor Request Receiver Interface
     * @param _port    Server Port
     */
    public TSocketServer(ISocketConnectionHandler _mserver, AppType _appType, int _port) {
        this.stopFlag = false;
        this.mWebSockets = _mserver;
        this.appType = _appType;
        this.port = _port;
        this.server = new SocketServer(this.port);
        this.server.start();
    }
    @Override
    public void run() {
        try {
            while(!this.stopFlag){
                Message message = this.mWebSockets.retrieveMessage(this.appType);
                this.server.broadcast(String.valueOf(message));
            }

        } catch (Exception e) {
            System.out.println(e);
        }
    }


    private class SocketServer extends WebSocketServer {
        private Set<WebSocket> conns;

        public SocketServer(int PORT) {
            super(new InetSocketAddress(PORT));
            conns = new HashSet<>();
        }
        public void broadcast(String message){
            for (WebSocket sock: conns){
                sock.send(message);
            }
        }
        @Override
        public void onOpen(WebSocket conn, ClientHandshake handshake) {
            conns.add(conn);
            System.out.println("New connection from " + conn.getRemoteSocketAddress().getAddress().getHostAddress());
        }

        @Override
        public void onClose(WebSocket conn, int code, String reason, boolean remote) {
            conns.remove(conn);
            System.out.println("Closed connection to " + conn.getRemoteSocketAddress().getAddress().getHostAddress());
        }

        @Override
        public void onMessage(WebSocket conn, String message) {
            System.out.println("Message from client: " + message);
            for (WebSocket sock : conns) {
                sock.send(message);
            }
        }

        @Override
        public void onError(WebSocket conn, Exception ex) {
            //ex.printStackTrace();
            if (conn != null) {
                conns.remove(conn);
            }
            System.out.println("ERROR from " + conn.getRemoteSocketAddress().getAddress().getHostAddress());
        }

        @Override
        public void onStart() {

        }
    }
}

