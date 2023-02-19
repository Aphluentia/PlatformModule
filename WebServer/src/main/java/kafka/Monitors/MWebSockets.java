package kafka.Monitors;

import kafka.Entities.*;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.Queue;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class MWebSockets implements IWebSocketHub, ISocketMessageHandler {

    private HashMap<AppType, Queue<String>> messages;
    /**
     * reentrant mutual exclusion lock
     */
    private final ReentrantLock rl;
    /**
     * Condition which indicates when there is space available in the CHILD Patients Room
     */
    private final Condition newMessage;




    /**
     * Generates the ETH monitor
     */
    public MWebSockets(){
        this.messages = new HashMap<AppType, Queue<String>>();
        for (AppType app: AppType.values()){
            this.messages.put(app, new LinkedList<>());
        }
        this.rl = new ReentrantLock();

        this.newMessage = rl.newCondition();

    }


    @Override
    public void addMessage(String newMessage, AppType appType) {
        try{
            rl.lock();
            this.messages.get(appType).add(newMessage);
            this.newMessage.signal();
        }finally {
            rl.unlock();
        }
    }


}