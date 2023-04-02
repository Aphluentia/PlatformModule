package kafka.Entities.Threads;

import com.google.gson.Gson;
import kafka.Entities.Enum.ComponentJList;
import kafka.Entities.Enum.GuiPanel;
import kafka.Entities.Enum.LogLevel;
import kafka.Entities.Enum.NumberLabel;
import kafka.Entities.Interfaces.GuiMonitor.IGui;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IModulesBroadcaster;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;
import kafka.Monitors.MLogger;
import org.java_websocket.WebSocket;


public class TModulesBroadcaster extends Thread {


    private boolean threadSuspended;
    private boolean stopFlag;
    private final MLogger mlogger;
    private final IModulesBroadcaster mModules;
    private final IGui mGui;

    /**
     * <b>Class Constructor</b>
     * <p>threadSuspended and stopFlag initialized as False</p>
     */
    public TModulesBroadcaster(IModulesBroadcaster _mModules, IGui _gui, MLogger _mlogger) {
        this.mlogger = _mlogger;
        this.mGui = _gui;
        this.mModules = _mModules;
        this.threadSuspended = false;
        this.stopFlag = false;
    }

    /**
     * <p>threadSuspended flag set to true, CCH waits for it to be false again</p>
     */
    public synchronized void suspendProcess() {
        this.threadSuspended = true;
    }

    /**
     * <p>threadSuspended flag set to false, suspended call centre is notified and resumes process</p>
     */
    public synchronized void resumeProcess() {
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
            while (!this.stopFlag) {
                synchronized (this) {
                    while (threadSuspended) {
                        try {
                            wait();
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                    }
                }
                // Add Code
                Message broadcastRequest = this.mModules.FetchMessage();
                mGui.removeMessage(ComponentJList.moduleInboundMessageList, broadcastRequest);
                mGui.numberOperation(GuiPanel.MODULES, NumberLabel.nModuleMessagesInQueue,"-");
                 if (this.mModules.IsValidBroadcastRequest(broadcastRequest)){
                    WebSocket broadcastConnection = this.mModules.FetchConnection(broadcastRequest.Source, broadcastRequest.Target);
                    broadcastConnection.send((new Gson()).toJson(broadcastRequest));
                    mGui.numberOperation(GuiPanel.MODULES, NumberLabel.nModuleMessagesBroadcasted,"+");
                     mGui.addMessage(ComponentJList.moduleBroadcastedMessageList, broadcastRequest);
                }else{
                     this.mModules.SignalRejectedMessage(broadcastRequest);
                     mGui.addMessage(ComponentJList.moduleDiscardedMessageList, broadcastRequest);
                    System.out.println("Rejected Request: " + new Gson().toJson(broadcastRequest));
                }
            }
        } catch (Exception e) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("TModulesBroadcaster Broadcasting Error %s", e)));
        }
    }
}