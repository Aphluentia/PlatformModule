package kafka.Entities.Threads;

import com.google.gson.Gson;
import kafka.Entities.Enum.*;
import kafka.Entities.Interfaces.GuiMonitor.IGui;
import kafka.Entities.Interfaces.SocketHubMonitor.ISocketHub;
import kafka.Entities.Models.ConnectionRequest;
import kafka.Entities.Models.ServerLog;
import kafka.Monitors.MLogger;
import kafka.guis.TServerGui;
import org.java_websocket.WebSocket;
import org.java_websocket.handshake.ClientHandshake;
import org.java_websocket.server.WebSocketServer;

import java.net.InetSocketAddress;
import java.util.HashMap;

public class TSocketHub extends Thread {
    private boolean stopFlag;
    private final boolean threadSuspended;
    private final WebSocketHub socketHub;
    private final MLogger mlogger;

    public TSocketHub(HashMap<ApplicationType, Integer> _typePorts, int _port, ISocketHub _mSocketHub, IGui _gui, MLogger _mlogger) {
        this.mlogger =_mlogger;
        this.stopFlag = false;
        this.threadSuspended = false;

        socketHub = new WebSocketHub(_port, _typePorts,_mSocketHub, _gui, _mlogger);
        socketHub.start();
    }
    /**
     * <p>stopFlag flag set to true, process ends</p>
     */
    public void stopProcess() {
        this.stopFlag = true;
    }
    @Override
    public void run() {
        try {
            while(!this.stopFlag) {
                synchronized (this) {
                    while (threadSuspended) {
                        try {
                            wait();
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                    }
                }
            }
            this.socketHub.stop();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }


    private static class WebSocketHub extends WebSocketServer {
        private final HashMap<ApplicationType, Integer> typePorts;
        private final MLogger mlogger;
        private final ISocketHub mSocketHub;
        private final IGui gui;
        public WebSocketHub(int PORT, HashMap<ApplicationType, Integer> _typePorts, ISocketHub _mSocketHub,IGui _gui, MLogger _mlogger) {
            super(new InetSocketAddress(PORT));
            this.gui = _gui;
            this.mlogger =_mlogger;
            this.typePorts = _typePorts;
            this.mSocketHub = _mSocketHub;
        }

        @Override
        public void onOpen(WebSocket conn, ClientHandshake handshake) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TWebSocketHub New Connection %s", conn.getRemoteSocketAddress().getAddress().getHostAddress())));

        }

        @Override
        public void onClose(WebSocket conn, int code, String reason, boolean remote) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TWebSocketHub Connection Closed %d %s: Client %s", code, reason,conn.getRemoteSocketAddress().getAddress().getHostAddress())));
        }

        @Override
        public void onMessage(WebSocket conn, String message) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TWebSocketHub Message Client %s: %s", conn.getRemoteSocketAddress().getAddress().getHostAddress(), message)));
            ConnectionRequest conReq = new Gson().fromJson(message, ConnectionRequest.class);

            TServerGui.revalidateSocketHubPanel();
            switch(conReq.Action){
                case CREATE_CONNECTION:
                    this.mSocketHub.AddNewSocketConnection(conReq.PlatformId, conn);
                    TServerGui.hubInboundConnectionRequestList.add(String.format("%s %s", conReq.PlatformId, conn.getRemoteSocketAddress().getHostString()));
                    gui.numberOperation(GuiPanel.SOCKET_HUB, NumberLabel.nHubConnections, "+");
                    break;
                case CLOSE_CONNECTION:
                    this.mSocketHub.CloseConnection(conReq.PlatformId, conn);
                    TServerGui.hubInboundConnectionRequestList.remove(String.format("%s %s", conReq.PlatformId, conn.getRemoteSocketAddress().getHostString()));
                    gui.numberOperation(GuiPanel.SOCKET_HUB, NumberLabel.nHubConnections, "-");
                    break;
                default:
                    conn.send("NOT_AVAILABLE");
            }
        }

        @Override
        public void onError(WebSocket conn, Exception ex) {
            if (conn!=null) this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("TWebSocketHub Socket Error %s - Client %s", ex, conn)));
        }

        @Override
        public void onStart() {
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TWebSocketHub Started at Port %s", ServerConfig.SOCKET_HUB_PORT)));
        }
    }
}