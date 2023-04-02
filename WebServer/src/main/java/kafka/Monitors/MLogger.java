package kafka.Monitors;
import kafka.Entities.Interfaces.ILogger;
import kafka.Entities.Models.ServerLog;

import java.util.LinkedList;
import java.util.Queue;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

/**
 * Logger Monitor
 */
public class MLogger implements ILogger {
    /**
     * reentrant mutual exclusion lock
     */
    private final ReentrantLock rl;
    /**
     * Condition which alerts the TLogger of a new log entry
     */
    private final Condition awaken;
    /**
     * Dynamic list of recent logs
     */
    private final Queue<ServerLog> logs;

    /**
     * Initializes the reentrantLock, the awaken condition and the logs MessageList
     */
    public MLogger(){
        this.rl  = new ReentrantLock();
        this.awaken = rl.newCondition();
        this.logs = new LinkedList<>();

    }


    /**
     * TLogger waits for a new log entry in the logs MessageList
     * When there is one, returns it and removes it from the MessageList
     * @return String new log entry
     */
    @Override
    public ServerLog waitForLog() {
        ServerLog toReturn = null;
        try{
            rl.lock();
            while(this.logs.isEmpty()) {
                try {
                    this.awaken.await();
                } catch (InterruptedException e) {
                    e.printStackTrace();
                }
            }
            toReturn = this.logs.remove();

        }finally {
            rl.unlock();
        }
        return toReturn;

    }
    /**
     * Adds a new log to the logs MessageList and notifies the TLogger
     */
    public void WriteLog(ServerLog message) {
        try{
            rl.lock();
            logs.add(message);
            this.awaken.signalAll();

        }finally {
            rl.unlock();
        }

    }
}