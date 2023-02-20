package kafka.Entities.Threads;

import kafka.Entities.AppType;
import kafka.Entities.ISocketConnectionHandler;
import org.java_websocket.WebSocket;
import org.java_websocket.handshake.ClientHandshake;
import org.java_websocket.server.WebSocketServer;

import java.io.*;
import java.net.InetSocketAddress;
import java.net.ServerSocket;
import java.net.Socket;
import java.util.HashSet;
import java.util.Set;

public class TWebSocketHub extends Thread {
    /**
     * MServer Monitor Request Receiver Interface
     */
    private final ISocketConnectionHandler mWebSockets;
    private final AppType appType;
    /**
     * Server Port
     */
    private int port;
    /**
     * Boolean flag for stopping process
     */
    private boolean stopFlag;

    /**
     * Initialize TServer
     *
     * @param _mserver MServer Monitor Request Receiver Interface
     * @param _port    Server Port
     */
    public TWebSocketHub(ISocketConnectionHandler _mserver, AppType _appType, int _port) {
        this.stopFlag = false;
        this.mWebSockets = _mserver;
        this.appType = _appType;
        this.port = _port;
        new WebsocketServer(this.port).start();


    }

    /**
     * <p>stopFlag flag set to true, process ends</p>
     */
    public void stopProcess() {
        this.stopFlag = true;

    }

    /**
     * Start running socket, send and receive information
     */
    @Override
    public void run() {



    }
}


class WebsocketServer extends WebSocketServer {
    private Set<WebSocket> conns;

    public WebsocketServer(int PORT) {
        super(new InetSocketAddress(PORT));
        conns = new HashSet<>();
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
            // do some thing if required
        }
        System.out.println("ERROR from " + conn.getRemoteSocketAddress().getAddress().getHostAddress());
    }

    @Override
    public void onStart() {

    }
}