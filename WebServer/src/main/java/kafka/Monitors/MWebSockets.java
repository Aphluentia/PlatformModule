package kafka.Monitors;

import kafka.Entities.*;
import kafka.Entities.Enum.AppType;
import kafka.Entities.Enum.LogLevel;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;

import java.util.*;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class MWebSockets implements ISocketConnectionHandler, ISocketMessageHandler {

    private final HashMap<AppType, Queue<Message>> messages;
    /**
     * reentrant mutual exclusion lock
     */
    private final ReentrantLock rl;
    /**
     * Condition which indicates when there is space available in the CHILD Patients Room
     */
    private final HashMap<AppType, Condition> newMessage;

    private final MLogger mlogger;


    /**
     * Generates the ETH monitor
     */
    public MWebSockets(MLogger _mlogger){
        this.mlogger = _mlogger;
        this.messages = new HashMap<AppType, Queue<Message>>();
        this.newMessage = new HashMap<AppType, Condition>();
        this.rl = new ReentrantLock();
        for (AppType app: AppType.values()){
            this.messages.put(app, new LinkedList<>());
            this.newMessage.put(app, rl.newCondition());
        }
    }


    @Override
    public void addMessage(Message newMessage, AppType appType) {
        try
        {
            rl.lock();
            this.messages.get(appType).add(newMessage);
            this.newMessage.get(appType).signal();
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, appType+": Adding Message to WebSocketsMonitor: "+newMessage));
        }
        finally 
        {
            rl.unlock();
        }
    }
    @Override
    public Message retrieveMessage(AppType appType) {
        Message message = null;
        try{
            rl.lock();
            while (this.messages.get(appType).isEmpty()){
                this.newMessage.get(appType).await();
            }
            message = this.messages.get(appType).remove();
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, appType+": Retrieving Message From WebSocketsMonitor: "+message));

        } catch (InterruptedException e) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, "Error When Retrieving Message From the WebSocketsMonitor: "+e));
            e.printStackTrace();
        } catch(NoSuchElementException e)
        {
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, "Error When Retrieving Message From the WebSocketsMonitor: "+e));
        }
        finally {
            rl.unlock();
        }
        return message;
    }

}