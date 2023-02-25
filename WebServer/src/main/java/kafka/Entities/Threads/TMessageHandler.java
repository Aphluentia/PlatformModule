package kafka.Entities.Threads;

import kafka.Entities.Enum.AppType;
import kafka.Entities.Enum.LogLevel;
import kafka.Entities.IMessageHandler;
import kafka.Entities.ISocketMessageHandler;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;
import kafka.Monitors.MLogger;

/**
 * Handles Messages Received from the KafkaConsumer
 */
public class TMessageHandler extends Thread{
    /**
     * All Monitor Call Center Interfaces -> Includes CCH, ETH, WTH and MDH
     */
    private final IMessageHandler imc;
    private final ISocketMessageHandler ismh;
    /**
     * Boolean flag for suspending process
     */
    private boolean threadSuspended;

    /**
     * Boolean Flag for stopping process
     */
    private boolean stopFlag;
    private final MLogger mlogger;
    /**
     * <b>Class Constructor</b>
     * <p>threadSuspended and stopFlag initialized as False</p>
     * @param _imc: Interface  for the MKafka Monitor
     */
    public TMessageHandler(IMessageHandler _imc, ISocketMessageHandler _ismh, MLogger _mlogger) {
        this.mlogger =_mlogger;
        this.imc = _imc;
        this.ismh = _ismh;
        this.threadSuspended = false;
        this.stopFlag = false;
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

    /**
     * <p>Run thread method</p>
     *<p>
     * While the process is running, the Call Centre keeps the simulation
     * running by receiving new SIGNAl signals.
     * If the result of the action produced by the received signal is unsatisfatory,
     * this last SIGNAL signal is added to the end of the message list in the CCH
     * </p>
     * <p> Unknown Signal launches an Error</p>
     */
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
                Message newMessage = this.imc.handleMessage();
                this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TMessageHandler Handling %s :%s: %s", newMessage.ApplicationType, newMessage.Action, newMessage.WebPlatformId)));
                this.ismh.addMessage(newMessage, Enum.valueOf(AppType.class, newMessage.ApplicationType));

            }
        } catch (Exception e) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("TMessageHandler Error %s",e)));
        }
    }
}
