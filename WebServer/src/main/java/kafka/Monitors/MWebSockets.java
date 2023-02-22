package kafka.Monitors;

import kafka.Entities.*;
import kafka.Entities.Enum.AppType;
import kafka.Entities.Models.Message;

import java.util.*;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class MWebSockets implements ISocketConnectionHandler, ISocketMessageHandler {

    private HashMap<AppType, Queue<Message>> messages;
    /**
     * reentrant mutual exclusion lock
     */
    private final ReentrantLock rl;
    /**
     * Condition which indicates when there is space available in the CHILD Patients Room
     */
    private final HashMap<AppType, Condition> newMessage;




    /**
     * Generates the ETH monitor
     */
    public MWebSockets(){
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
        try{
            rl.lock();
            this.messages.get(appType).add(newMessage);
            System.out.println(newMessage);
            this.newMessage.get(appType).signal();
        }finally {
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

        } catch (InterruptedException e) {
            e.printStackTrace();
        } catch(NoSuchElementException e)
        {

        }finally {
            rl.unlock();
        }
        return message;
    }

}