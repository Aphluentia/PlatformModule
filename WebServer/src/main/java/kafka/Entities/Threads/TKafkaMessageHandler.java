package kafka.Entities.Threads;

import kafka.Entities.Enum.*;
import kafka.Entities.Interfaces.GuiMonitor.IGui;
import kafka.Entities.Interfaces.KafkaMonitor.IKafkaMessageHandler;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IKafkaModulesMessageHandler;
import kafka.Entities.Interfaces.SocketHubMonitor.ISocketHubKafkaMessageHandler;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;
import kafka.Monitors.MLogger;
import kafka.guis.TServerGui;
import org.java_websocket.WebSocket;

/**
 * Handles Messages Received from the KafkaConsumer
 */
public class TKafkaMessageHandler extends Thread{
    /**
     * All Monitor Call Center Interfaces -> Includes CCH, ETH, WTH and MDH
     */
    private final IKafkaMessageHandler mKafka;
    private final IKafkaModulesMessageHandler mModules;
    private final ISocketHubKafkaMessageHandler mSocketHub;
    /**
     * Boolean flag for suspending process
     */
    private boolean threadSuspended;

    /**
     * Boolean Flag for stopping process
     */
    private boolean stopFlag;
    private final MLogger mlogger;
    private final IGui gui;

    public TKafkaMessageHandler(IKafkaMessageHandler _mKafka, IKafkaModulesMessageHandler _mModules,
                                ISocketHubKafkaMessageHandler _mSocketHub, IGui _gui, MLogger _mlogger)
    {
        this.mlogger =_mlogger;
        this.mKafka = _mKafka;
        this.gui = _gui;
        this.mModules = _mModules;
        this.mSocketHub = _mSocketHub;
        this.threadSuspended = false;
        this.stopFlag = false;
    }

    public synchronized void suspendProcess(){
        this.threadSuspended = true;
    }

    public synchronized void resumeProcess(){
        this.threadSuspended = false;
        notify();
    }

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
                Message newMessage = this.mKafka.FetchMessage();
                gui.numberOperation(GuiPanel.KAFKA, NumberLabel.nMessagesFetched, "+");
                gui.removeMessage(ComponentJList.kafkaInboundMessagesList, newMessage);
                this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TMessageHandler Handling %s :%s: %s", newMessage.Source, newMessage.Action, newMessage.Target)));

                switch(newMessage.Action){
                    case CREATE_CONNECTION:
                        // Só vem dos módulos se a conexão foi criada
                        if (this.mSocketHub.IsValidTargetConnection(newMessage.Target))
                        {
                            System.out.println("Checkpoint: Connection Created "+newMessage.Target +" from "+ newMessage.Source);
                            this.mSocketHub.BroadcastNewModuleConnectionRequest(newMessage);
                            gui.numberOperation(GuiPanel.SOCKET_HUB, NumberLabel.nHubConnections, "+");
                            this.mModules.AddNewModule(newMessage);
                        }
                        break;
                    case CLOSE_CONNECTION:
                        // Só vem dos módulos se a conexão foi fechada

                        break;

                    case PING:
                        this.mModules.AddNewMessage(newMessage);
                        TServerGui.hubInboundMessagesList.add(newMessage);
                        gui.numberOperation(GuiPanel.SOCKET_HUB, NumberLabel.nHubMessagesReceived, "+");
                        gui.numberOperation(GuiPanel.SOCKET_HUB, NumberLabel.nHubMessagesInQueue, "+");
                        break;
                    case UPDATE:
                        this.mModules.AddNewMessage(newMessage);
                        TServerGui.hubInboundMessagesList.add(newMessage);
                        gui.numberOperation(GuiPanel.SOCKET_HUB, NumberLabel.nHubMessagesReceived, "+");
                        gui.numberOperation(GuiPanel.SOCKET_HUB, NumberLabel.nHubMessagesInQueue, "+");
                        break;

                    default:

                        break;
                }

            }
        } catch (Exception e) {
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("TMessageHandler Error %s",e)));
        }
    }
}
