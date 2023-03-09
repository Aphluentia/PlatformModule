package kafka.Entities.Threads;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.ConnectionAction;
import kafka.Entities.Enum.LogLevel;
import kafka.Entities.Interfaces.KafkaMonitor.IKafkaMessageHandler;
import kafka.Entities.Interfaces.ModulesSocketServerMonitor.IKafkaModulesMessageHandler;
import kafka.Entities.Interfaces.SocketHubMonitor.ISocketHubKafkaMessageHandler;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;
import kafka.Monitors.MLogger;
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

    public TKafkaMessageHandler(IKafkaMessageHandler _mKafka, IKafkaModulesMessageHandler _mModules,
                                ISocketHubKafkaMessageHandler _mSocketHub, MLogger _mlogger)
    {
        this.mlogger =_mlogger;
        this.mKafka = _mKafka;
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
                this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TMessageHandler Handling %s :%s: %s", newMessage.Source, newMessage.Action, newMessage.Target)));

                switch(newMessage.Action){
                    case CREATE_CONNECTION:
                        // Só vem dos módulos se a conexão foi criada
                        if (this.mSocketHub.IsValidTargetConnection(newMessage.Target))
                        {
                            this.mSocketHub.BroadcastNewModuleConnectionRequest(newMessage);
                            this.mModules.AddNewModule(newMessage);
                        }
                        break;
                    case CLOSE_CONNECTION:
                        // Só vem dos módulos se a conexão foi fechada

                        break;

                    case PING:
                        this.mModules.AddNewMessage(newMessage);
                        break;
                    case UPDATE:
                        this.mModules.AddNewMessage(newMessage);
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
