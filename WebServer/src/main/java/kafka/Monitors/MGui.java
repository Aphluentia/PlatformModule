package kafka.Monitors;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.ComponentJList;
import kafka.Entities.Enum.GuiPanel;
import kafka.Entities.Enum.NumberLabel;
import kafka.Entities.Interfaces.GuiMonitor.IGui;
import kafka.Entities.Models.ConnectionRequest;
import kafka.Entities.Models.Message;
import kafka.guis.TServerGui;

import java.util.Queue;
import java.util.concurrent.locks.Condition;
import java.util.concurrent.locks.ReentrantLock;

public class MGui implements IGui {

    /**
     * reentrant mutual exclusion lock
     */
    private final ReentrantLock rl;


    /**
     * Generates the ETH monitor
     */
    public MGui(){
        rl = new ReentrantLock();
    }

    @Override
    public void numberOperation(GuiPanel panel, NumberLabel number, String action) {
        try{
            rl.lock();
            int op = (action == "+" ? 1 : -1);
            switch(number){
                case nMessagesReceived:
                    TServerGui.nMessagesReceived+=op;
                    break;
                case nMessagesFetched:
                    TServerGui.nMessagesFetched += op;
                    break;
                case nHubConnections:
                    TServerGui.nHubConnections += op;
                    break;
                case nHubMessagesReceived:
                    TServerGui.nHubMessagesReceived += op;
                    break;
                case nHubMessagesInQueue:
                    TServerGui.nHubMessagesInQueue += op;
                    break;
                case nHubMessagesBroadcasted:
                    TServerGui.nHubMessagesBroadcasted += op;
                    break;
                case nModuleMessagesReceived:
                    TServerGui.nModuleMessagesReceived += op;
                    break;
                case nModuleMessagesInQueue:
                    TServerGui.nModuleMessagesInQueue += op;
                    break;
                case nModuleMessagesBroadcasted:
                    TServerGui.nModuleMessagesBroadcasted += op;
                    break;
                case nModuleMessagesDiscarded:
                    TServerGui.nModuleMessagesDiscarded += op;
                    break;
            }
            switch(panel){
                case KAFKA:
                    TServerGui.revalidateKafkaPanel();
                    break;
                case MODULES:
                    TServerGui.revalidateModulesPanel();
                    break;
                case SOCKET_HUB:
                    TServerGui.revalidateSocketHubPanel();
                    break;
            }
        }finally {
            rl.unlock();
        }

    }

    @Override
    public void addMessage(ComponentJList jlist, Message message) {
        try{
            rl.lock();
            switch (jlist){
                case kafkaInboundMessagesList:
                    TServerGui.kafkaInboundMessagesList.add(message);
                    TServerGui.revalidateKafkaPanel();
                    break;
                case hubInboundMessagesList:
                    TServerGui.hubInboundMessagesList.add(message);
                    TServerGui.revalidateSocketHubPanel();
                    break;
                case moduleInboundMessageList:
                    TServerGui.moduleInboundMessageList.add(message);
                    TServerGui.revalidateModulesPanel();
                    break;
                case moduleDiscardedMessageList:
                    TServerGui.moduleDiscardedMessageList.add(message);
                    TServerGui.revalidateModulesPanel();
                    break;
                case moduleBroadcastedMessageList:
                    TServerGui.moduleBroadcastedMessageList.add(message);
                    TServerGui.revalidateModulesPanel();
                    break;
            }

        }finally {
            rl.unlock();
        }
    }

    @Override
    public void removeMessage(ComponentJList jlist, Message message) {
        try{
            rl.lock();
            switch (jlist){
                case kafkaInboundMessagesList:
                    TServerGui.kafkaInboundMessagesList.remove(message);
                    TServerGui.revalidateKafkaPanel();
                    break;
                case hubInboundMessagesList:
                    TServerGui.hubInboundMessagesList.remove(message);
                    TServerGui.revalidateSocketHubPanel();
                    break;
                case moduleInboundMessageList:
                    TServerGui.moduleInboundMessageList.remove(message);
                    TServerGui.revalidateModulesPanel();
                    break;
                case moduleDiscardedMessageList:
                    TServerGui.moduleDiscardedMessageList.remove(message);
                    TServerGui.revalidateModulesPanel();
                    break;
                case moduleBroadcastedMessageList:
                    TServerGui.moduleBroadcastedMessageList.remove(message);
                    TServerGui.revalidateModulesPanel();
                    break;
            }

        }finally {
            rl.unlock();
        }
    }

    @Override
    public void addConnectionRequest(ComponentJList jlist, ApplicationType appType, ConnectionRequest message) {
        try{
            rl.lock();
            switch (jlist){
                case hubInboundConnectionRequestList:
                    TServerGui.hubInboundConnectionRequestList.add(String.valueOf(message));
                    TServerGui.revalidateSocketHubPanel();
                    break;
                case connectionsList:
                    TServerGui.connectionsList.get(appType).add(String.valueOf(message));
                    TServerGui.revalidateModulesPanel();
                    break;

            }

        }finally {
            rl.unlock();
        }
    }

    @Override
    public void removeConnectionRequest(ComponentJList jlist, ApplicationType appType, ConnectionRequest message) {
        try{
            rl.lock();
            switch (jlist){
                case hubInboundConnectionRequestList:
                    TServerGui.hubInboundConnectionRequestList.remove(String.valueOf(message));
                    TServerGui.revalidateSocketHubPanel();
                    break;
                case connectionsList:
                    TServerGui.connectionsList.get(appType).remove(String.valueOf(message));
                    TServerGui.revalidateModulesPanel();
                    break;

            }

        }finally {
            rl.unlock();
        }
    }
}
