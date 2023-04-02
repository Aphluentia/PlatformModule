package kafka.Monitors;

import com.google.gson.Gson;
import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Interfaces.KafkaMonitor.IKafkaMessageHandler;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IKafkaModulesMessageHandler;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IModuleRejectedMessageHandler;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IModules;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IModulesBroadcaster;
import kafka.Entities.Models.ConnectionRequest;
import kafka.Entities.Models.Message;
import kafka.guis.TServerGui;
import org.java_websocket.WebSocket;
import org.slf4j.event.KeyValuePair;

import java.util.*;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class MModules implements IKafkaModulesMessageHandler, IModules, IModulesBroadcaster, IModuleRejectedMessageHandler {

    private final Queue<Message> messages;
    private Queue<Message> rejected;
    public HashMap<String, HashMap<String, WebSocket>> connections;
    /**
     * reentrant mutual exclusion lock
     */
    private final ReentrantLock rl;
    /**
     * Condition which indicates when there is space available in the CHILD Patients Room
     */
    private final Condition newMessage, newRejectedMessage;

    private final MLogger mlogger;


    /**
     * Generates the ETH monitor
     */
    public MModules(MLogger _mlogger){
        this.mlogger = _mlogger;
        this.messages = new LinkedList<Message>();
        this.rejected = new LinkedList<Message>();
        this.rl = new ReentrantLock();
        this.newRejectedMessage = rl.newCondition();
        this.newMessage = rl.newCondition();
        this.connections = new HashMap<>();


    }




    @Override
    public void AddNewMessage(Message newMessage) {
        try{
            rl.lock();
            this.messages.add(newMessage);

            this.newMessage.signal();
        }finally {
            rl.unlock();
        }
    }

    @Override
    public void AddNewModule(Message newMessage) {
        try{
            rl.lock();

            if (!this.connections.containsKey(newMessage.Source))
                this.connections.put(newMessage.Source, new HashMap<String, WebSocket>());
            System.out.println(this.connections.keySet());
        }finally {
            rl.unlock();
        }
    }


    // Add a new platform connection to module
    @Override
    public void AddNewSocketConnection(ConnectionRequest request, WebSocket conn) {
        try{
            rl.lock();
            this.connections.get(request.ModuleId).put(request.PlatformId, conn);
        }finally {
            rl.unlock();
        }
    }

    @Override
    public WebSocket CloseConnection(ConnectionRequest request, WebSocket conn) {
        WebSocket value = null;
        try{
            rl.lock();
            if (this.connections.get(request.ModuleId).containsKey(request.PlatformId))
                value = this.connections.get(request.ModuleId).remove(request.PlatformId);
        }finally {
            rl.unlock();
        }
        return value;
    }
    // Fetch New Kafka Message to be broadcasted
    @Override
    public Message FetchMessage() {
        Message fetched = null;
        try{
            rl.lock();
            while(this.messages.isEmpty())
                this.newMessage.await();
            fetched = this.messages.remove();
        } catch (InterruptedException e) {
            throw new RuntimeException(e);
        } finally {
            rl.unlock();
        }
        return fetched;
    }



    @Override
    public boolean IsValidBroadcastRequest(Message broadcastRequest) {
        boolean IsValid = false;
        WebSocket conn = null;
        try{
            rl.lock();

            IsValid =  this.connections.containsKey(broadcastRequest.Source) && this.connections.get(broadcastRequest.Source).containsKey(broadcastRequest.Target);
        } finally {
            rl.unlock();
        }
        return IsValid;
    }
    // Fetch the WebSocket Platform Connection
    @Override
    public WebSocket FetchConnection(String Source, String Target) {
        WebSocket conn = null;
        try{
            rl.lock();
            conn = this.connections.get(Source).get(Target);
        } finally {
            rl.unlock();
        }
        return conn;
    }

    @Override
    public void SignalRejectedMessage(Message broadcastRequest) {

        try{
            rl.lock();
            this.rejected.add(broadcastRequest);
            this.newRejectedMessage.signal();
        } finally {
            rl.unlock();
        }
    }

    @Override
    public void InsertRejectedMessage(Message message) {
        try{
            rl.lock();
            this.messages.add(message);
            this.newMessage.signal();
        } finally {
            rl.unlock();
        }
    }

    @Override
    public Message FetchRejectedMessage() {
        Message value = null;
        try{
            rl.lock();
            while(this.rejected.isEmpty()){
                this.newRejectedMessage.wait();
            }
            value = this.rejected.remove();
        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            rl.unlock();
        }
        return value;
    }


}