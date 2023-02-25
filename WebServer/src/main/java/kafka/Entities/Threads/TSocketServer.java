package kafka.Entities.Threads;

import kafka.Entities.Enum.AppType;
import kafka.Entities.Enum.LogLevel;
import kafka.Entities.ISocketConnectionHandler;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;
import kafka.Monitors.MLogger;
import org.java_websocket.WebSocket;
import org.java_websocket.handshake.ClientHandshake;
import org.java_websocket.server.WebSocketServer;

import java.net.InetSocketAddress;
import java.util.HashSet;
import java.util.Set;

public class TSocketServer extends Thread {
    private final ISocketConnectionHandler mWebSockets;
    private final AppType appType;
    private boolean stopFlag;
    private boolean threadSuspended;
    private final SocketServer server;
    private final MLogger mlogger;
    /**
     * Initialize TServer
     *
     * @param _mserver MServer Monitor Request Receiver Interface
     * @param _port    Server Port
     */
    public TSocketServer(ISocketConnectionHandler _mserver, AppType _appType, int _port, MLogger _mlogger) {
        this.mlogger =_mlogger;
        this.stopFlag = false;
        this.mWebSockets = _mserver;
        this.appType = _appType;
        this.server = new SocketServer(_port, _appType, _mlogger);
        this.server.start();
    }
    /**
     * <p>threadSuspended flag set to true, CCH waits for it to be false again</p>
     */
    public synchronized void suspendProcess(){
        this.threadSuspended = true;
    }
    /**
     * <p>threadSuspended flag set to false, suspended call centre is notified and resumes process</p>
     */
    public synchronized void resumeProcess(){
        this.threadSuspended = false;
        notify();
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
            while(!this.stopFlag){
                synchronized(this) {
                    while (threadSuspended) {
                        try {
                            wait();
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                    }
                }
                Message message = this.mWebSockets.retrieveMessage(this.appType);
                this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TSocketServer Message Sent %s :%s: %s",this.appType,message.Action, message.WebPlatformId)));
                this.server.broadcast(String.valueOf(message));
            }

        } catch (Exception e) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("TSocketServer Error %s: %s ",this.appType,e)));

        }
    }


    private static class SocketServer extends WebSocketServer {
        private final Set<WebSocket> conns;
        private final MLogger mlogger;
        private final AppType appType;

        public SocketServer(int PORT, AppType _appType, MLogger _mlogger) {
            super(new InetSocketAddress(PORT));
            this.appType = _appType;
            this.mlogger = _mlogger;
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
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TSocketServer New Connection %s from Client %s", this.appType, conn.getRemoteSocketAddress().getAddress().getHostAddress())));

        }

        @Override
        public void onClose(WebSocket conn, int code, String reason, boolean remote) {
            conns.remove(conn);
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TSocketServer Closed Connection to Server %s %d:%s - Client %s",this.appType, code, reason, conn.getRemoteSocketAddress().getAddress().getHostAddress() )));
        }

        @Override
        public void onMessage(WebSocket conn, String message) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TSocketServer New Message From Client %s: %s - Client %s",this.appType, message, conn.getRemoteSocketAddress().getAddress().getHostAddress())));

        }

        @Override
        public void onError(WebSocket conn, Exception ex) {
            //ex.printStackTrace();
            if (conn != null) {
                conns.remove(conn);
            }
            assert conn != null;
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("TSocketServer: Error on Receiving Message %s from Client %s", ex, conn.getRemoteSocketAddress().getAddress().getHostAddress())));
        }

        @Override
        public void onStart() {

        }
    }
}

