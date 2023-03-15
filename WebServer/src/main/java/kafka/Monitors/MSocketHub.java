package kafka.Monitors;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.LogLevel;
import kafka.Entities.Interfaces.SocketHubMonitor.IHubBroadcaster;
import kafka.Entities.Interfaces.SocketHubMonitor.ISocketHub;
import kafka.Entities.Interfaces.SocketHubMonitor.ISocketHubKafkaMessageHandler;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;
import kafka.guis.TServerGui;
import org.java_websocket.WebSocket;

import java.util.*;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class MSocketHub implements IHubBroadcaster, ISocketHub, ISocketHubKafkaMessageHandler {

    private HashMap<String, WebSocket> platformConnections;
    /**
     * reentrant mutual exclusion lock
     */
    private final ReentrantLock rl;


    private final MLogger mlogger;
    private final Condition newBroadcastRequest;
    private Queue<Message> broadcastRequests;


    /**
     * Generates the ETH monitor
     */
    public MSocketHub(MLogger _mlogger){
        this.mlogger = _mlogger;
        this.platformConnections = new HashMap<>();
        this.broadcastRequests = new LinkedList<>();
        this.rl = new ReentrantLock();
        this.newBroadcastRequest = rl.newCondition();

    }


    @Override
    public boolean IsValidTargetConnection(String Target) {
        boolean isValid = false;
        try
        {
            rl.lock();
            isValid = this.platformConnections.containsKey(Target);
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("Someone's Trying to Connect to %s, is Connection Valid: %s", Target, isValid)));
        }
        finally
        {
            rl.unlock();
        }
        return isValid;
    }

    // Discovery: Show Available Ports
    @Override
    public void BroadcastNewModuleConnectionRequest(Message newMessage) {
        try
        {
            rl.lock();
            broadcastRequests.add(newMessage);
            this.newBroadcastRequest.signal();
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("Added New Module Connection %s to %s", newMessage.Source, newMessage.Target)));
        }
        finally
        {
            rl.unlock();
        }
    }

    // Wait for New Broadcast Request
    @Override
    public Message FetchForBroadcastRequest() {
        Message broadcastReq = null;
        try
        {
            rl.lock();
            while(this.broadcastRequests.isEmpty()){
                this.newBroadcastRequest.await();
            }
            broadcastReq = this.broadcastRequests.remove();
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("Received New Broadcast Request From %s to %s", broadcastReq.Source, broadcastReq.Target)));
        } catch (InterruptedException e) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("Error Waiting For Broadcast Request: %s", e)));

        } finally
        {
            rl.unlock();
        }
        return broadcastReq;
    }

    @Override
    public WebSocket FetchConnection(String Target) {
        WebSocket conn = null;
        try
        {
            rl.lock();
            if (this.platformConnections.containsKey(Target))
                conn = this.platformConnections.get(Target);
        } finally
        {
            rl.unlock();
        }
        return conn;
    }

    // Add New Hub Connection to Records
    @Override
    public void AddNewSocketConnection(String PlatformId, WebSocket conn) {
        try
        {
            rl.lock();
            this.platformConnections.put(PlatformId, conn);
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("Added New Connection %s to %s", PlatformId, conn.getRemoteSocketAddress().getAddress().getHostAddress())));
        }
        finally
        {
            rl.unlock();
        }
    }

    @Override
    public void CloseConnection(String PlatformId, WebSocket conn) {
        conn.close();
        try
        {
            rl.lock();
            this.platformConnections.remove(PlatformId);
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("Removed Connection %s to %s", PlatformId, conn.getRemoteSocketAddress().getAddress().getHostAddress())));
        }
        finally
        {
            rl.unlock();
        }
    }
}