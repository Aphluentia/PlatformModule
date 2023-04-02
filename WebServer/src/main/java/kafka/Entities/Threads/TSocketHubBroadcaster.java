package kafka.Entities.Threads;

import com.google.gson.Gson;
import kafka.Entities.Enum.*;
import kafka.Entities.Interfaces.GuiMonitor.IGui;
import kafka.Entities.Interfaces.SocketHubMonitor.IHubBroadcaster;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;
import kafka.Entities.Models.ServerResponse;
import kafka.Monitors.MLogger;
import kafka.guis.TServerGui;
import org.java_websocket.WebSocket;

import java.sql.SQLOutput;
import java.util.HashMap;
import java.util.Properties;

public class TSocketHubBroadcaster extends Thread{


    private boolean threadSuspended;
    private boolean stopFlag;
    private final MLogger mlogger;
    private final HashMap<ApplicationType, Integer> typePorts;
    private final IHubBroadcaster mSocketHub;
    private final IGui gui;
    /**
     * <b>Class Constructor</b>
     * <p>threadSuspended and stopFlag initialized as False</p>
     */
    public TSocketHubBroadcaster(IHubBroadcaster _mSocketHub, IGui _gui, HashMap<ApplicationType, Integer> _typePorts, MLogger _mlogger) {
        this.mlogger =_mlogger;
        this.gui = _gui;
        this.mSocketHub = _mSocketHub;
        this.typePorts = _typePorts;
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
               // Add Code
                Message broadcastRequest = this.mSocketHub.FetchForBroadcastRequest();
                WebSocket conn = this.mSocketHub.FetchConnection(broadcastRequest.Target);
                TServerGui.hubInboundMessagesList.remove(broadcastRequest);
                gui.numberOperation(GuiPanel.SOCKET_HUB, NumberLabel.nHubMessagesInQueue, "-");
                gui.removeMessage(ComponentJList.hubInboundMessagesList, broadcastRequest);

                if (broadcastRequest.Action == ConnectionAction.CREATE_CONNECTION){
                    ServerResponse res = new ServerResponse(broadcastRequest.Source, broadcastRequest.Target, broadcastRequest.SourceApplicationType,
                            typePorts.get(broadcastRequest.SourceApplicationType).toString());
                    conn.send((new Gson()).toJson(res));
                    gui.numberOperation(GuiPanel.SOCKET_HUB, NumberLabel.nHubMessagesBroadcasted, "+");
                }

            }
        }
        catch (Exception e)
        {
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("THubBroadcaster Broadcasting Error %s",e)));
        }
    }
}
