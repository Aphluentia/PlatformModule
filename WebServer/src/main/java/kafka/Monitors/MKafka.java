package kafka.Monitors;

import kafka.Entities.Enum.LogLevel;
import kafka.Entities.IKafkaConsumer;
import kafka.Entities.IMessageHandler;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;

import java.util.LinkedList;
import java.util.Queue;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class MKafka implements IKafkaConsumer, IMessageHandler {
    private final Queue<Message> messages;
    /**
     * reentrant mutual exclusion lock
     */
    private final ReentrantLock rl;
    /**
     * Condition which indicates when there is space available in the CHILD Patients Room
     */
    private final Condition newMessage;
    
    private final MLogger mlogger;
    /**
     * Generates the ETH monitor
     */
    public MKafka(MLogger _mlogger){
        this.mlogger = _mlogger;
        this.messages = new LinkedList<Message>();
        this.rl = new ReentrantLock();
        this.newMessage = rl.newCondition();
    }


    @Override
    public void addMessage(Message newMessage) {
        try{
            rl.lock();
            this.messages.add(newMessage);
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, "Adding Message to KafkaMonitor: "+newMessage));
            this.newMessage.signal();
        }finally {
            rl.unlock();
        }
    }

    @Override
    public Message handleMessage() {
        Message message = null;
        try{
            rl.lock();
            while (this.messages.isEmpty()){
                this.newMessage.await();
            }
            message = this.messages.remove();
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, "Processing Message: "+message));
        } catch (InterruptedException e) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, "KafkaMonitorException: "+e));
        } finally {
            rl.unlock();
        }
        return message;
    }
}