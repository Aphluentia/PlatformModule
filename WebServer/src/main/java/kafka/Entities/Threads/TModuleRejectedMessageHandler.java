package kafka.Entities.Threads;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.LogLevel;
import kafka.Entities.Enum.ServerConfig;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IModuleRejectedMessageHandler;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IModules;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;
import kafka.Monitors.MLogger;

import java.util.Date;

public class TModuleRejectedMessageHandler extends Thread {
    private final IModuleRejectedMessageHandler mModules;
    private boolean stopFlag;
    private boolean threadSuspended;
    private final MLogger mlogger;
    /**
     * Initialize TServer
     *
     * @param _mModules    Server Port
     */
    public TModuleRejectedMessageHandler(IModuleRejectedMessageHandler _mModules, MLogger _mlogger) {
        this.mlogger =_mlogger;
        this.stopFlag = false;
        this.mModules = _mModules;
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
                Message message = this.mModules.FetchRejectedMessage();
                long currentTimeInSeconds = System.currentTimeMillis() / 1000;
                if ((new Date(message.Timestamp)).toInstant().plusSeconds( ServerConfig.VALIDITY).isBefore(new Date().toInstant()) ){
                    wait(ServerConfig.TIME_WAIT_BEFORE_REINSERTION);
                    this.mModules.InsertRejectedMessage(message);
                }else{
                    this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, "Rejected message has expired"));
                }
            }
        } catch (Exception e) {

        }
    }
}
