package kafka.Monitors;

import kafka.Entities.IKafkaConsumer;
import kafka.Entities.IMessageHandler;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.Queue;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class MKafka implements IKafkaConsumer, IMessageHandler {

    private Queue<String> messages;
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
    public MKafka(){
        this.messages = new LinkedList<String>();
        this.rl = new ReentrantLock();

        this.newMessage = rl.newCondition();

    }


    @Override
    public void addMessage(String newMessage) {
        try{
            rl.lock();
            this.messages.add(newMessage);
            this.newMessage.signal();
        }finally {
            rl.unlock();
        }
    }

    @Override
    public String handleMessage() {
        String message = "";
        try{
            rl.lock();
            while (this.messages.isEmpty()){
                this.newMessage.await();
            }
            message = this.messages.remove();

        } catch (InterruptedException e) {
            e.printStackTrace();
        } finally {
            rl.unlock();
        }
        return message;
    }
}