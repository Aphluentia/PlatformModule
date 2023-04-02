package kafka.Entities.Threads;

import com.google.gson.Gson;
import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.ComponentJList;
import kafka.Entities.Enum.LogLevel;
import kafka.Entities.Interfaces.GuiMonitor.IGui;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IModules;
import kafka.Entities.Models.ConnectionRequest;
import kafka.Entities.Models.ServerLog;
import kafka.Monitors.MLogger;
import org.java_websocket.WebSocket;
import org.java_websocket.handshake.ClientHandshake;
import org.java_websocket.server.WebSocketServer;

import java.net.InetSocketAddress;

public class TModulesSocketServer extends Thread {
    private final IModules mModules;
    private final ApplicationType appType;
    private boolean stopFlag;
    private boolean threadSuspended;
    private final SocketServer server;
    private final MLogger mlogger;
    private final IGui mGui;
    /**
     * Initialize TServer
     *
     * @param _port    Server Port
     */
    public TModulesSocketServer(IModules _mModules, IGui _gui, ApplicationType _appType, int _port, MLogger _mlogger) {
        this.mlogger =_mlogger;
        this.stopFlag = false;
        this.mGui = _gui;
        this.mModules = _mModules;
        this.appType = _appType;
        this.server = new SocketServer(_port, _appType, _gui, _mModules, _mlogger);
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
            }

        } catch (Exception e) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("TSocketServer Error %s: %s ",this.appType,e)));
        }
    }

    private static class SocketServer extends WebSocketServer {
        private final MLogger mlogger;
        private final ApplicationType appType;
        private final IModules mModules;
        private final IGui mGui;
        public SocketServer(int PORT, ApplicationType _appType,IGui _gui, IModules _mModules, MLogger _mlogger) {
            super(new InetSocketAddress(PORT));
            this.appType = _appType;
            this.mGui = _gui;
            this.mModules = _mModules;
            this.mlogger = _mlogger;
        }

        @Override
        public void onOpen(WebSocket conn, ClientHandshake handshake) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TSocketServer New Connection %s from Client %s", this.appType, conn.getRemoteSocketAddress().getAddress().getHostAddress())));
        }

        @Override
        public void onClose(WebSocket conn, int code, String reason, boolean remote) {

            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TSocketServer Closed Connection to Server %s %d:%s - Client %s",this.appType, code, reason, conn.getRemoteSocketAddress().getAddress().getHostAddress() )));
        }

        @Override
        public void onMessage(WebSocket conn, String message) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TSocketServer New Message From Client %s: %s - Client %s",this.appType, message, conn.getRemoteSocketAddress().getAddress().getHostAddress())));
            ConnectionRequest conReq = new Gson().fromJson(message, ConnectionRequest.class);

            System.out.println("Checkpoint Module: "+ conReq.Action+" from " +conReq.PlatformId);
            switch(conReq.Action){
                case CREATE_CONNECTION:
                    System.out.println("Creating New Connection");
                    this.mModules.AddNewSocketConnection(conReq, conn);
                    System.out.println("Checkpoint Module: New Connection "+conn.getRemoteSocketAddress().toString()+" Added "+conReq.PlatformId);
                    mGui.addConnectionRequest(ComponentJList.connectionsList, appType, conReq);
                    break;
                case CLOSE_CONNECTION:
                    this.mModules.CloseConnection(conReq, conn);
                    mGui.removeConnectionRequest(ComponentJList.connectionsList, appType, conReq);
                    break;
                default:
                    conn.send("NOT_AVAILABLE");
            }
        }

        @Override
        public void onError(WebSocket conn, Exception ex) {
            //ex.printStackTrace();

            if(conn != null)
                this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("TSocketServer: Error on Receiving Message %s from Client %s", ex, conn.getRemoteSocketAddress().getAddress().getHostAddress())));
        }

        @Override
        public void onStart() {

        }
    }
}

