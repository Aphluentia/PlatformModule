package kafka.guis;

import kafka.App;
import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Models.ConnectionRequest;
import kafka.Entities.Models.Message;

import javax.swing.*;
import java.awt.*;
import java.util.*;

public class TServerGui extends Thread{
    public static JFrame jframe;
    public static JPanel KafkaPanel, SocketHubPanel, ModulesPanel, ModulesJListPanel;


    //Panels: SocketHubPanel, (KafkaPanel, MessageHandler), AppTypePanel
    public TServerGui(){
        TServerGui.jframe = new JFrame("Web Server");
        kafkaInboundMessagesList = new ArrayList<>();
        hubInboundMessagesList = new ArrayList<>();
        hubInboundConnectionRequestList = new ArrayList<>();

        moduleInboundMessageList = new ArrayList<>();
        moduleDiscardedMessageList = new ArrayList<>();
        moduleBroadcastedMessageList = new ArrayList<>();
        connectionScrollPanel = new HashMap<>();
        connectionsList = new HashMap<>();
        for (ApplicationType type: ApplicationType.values()) {
            if (type != ApplicationType.WEB_PLATFORM) {
                connectionsList.put(type, new ArrayList<>());
            }
        }
        connectionsJList = new HashMap<>();

        TServerGui.jframe.getContentPane().setLayout(new GridLayout(4,2));

        setKafkaPanel();
        setSocketHubPanel();
        setModulesPanel();
        //generateMessagesPanel();    //(KafkaPanel, MessageHandler)
        //generateSocketHubPanel();
        //generateAppTypePanel();
        TServerGui.jframe.setSize(1200,1600);
        TServerGui.jframe.setVisible(true);

    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Kafka Panel /////////////////////////////////////////////////////////////////////////////////////////////////////
    public static int nMessagesReceived, nMessagesInQueue, nMessagesFetched;
    public static JLabel nMessagesReceivedLabel, nMessagesInQueueLabel, nMessagesFetchedLabel;
    public static ArrayList<Message> kafkaInboundMessagesList;
    public static JScrollPane kafkaInboundMessagesScrollPane;
    public static JList<Message> kafkaInboundMessagesJList;
    // Create a Kafka Panel
    public static void setKafkaPanel(){
        KafkaPanel = new JPanel();

        KafkaPanel.setLayout(new BoxLayout(KafkaPanel, BoxLayout.Y_AXIS));
        KafkaPanel.setBorder(BorderFactory.createTitledBorder("Kafka Monitor"));

        KafkaPanel.add(nMessagesReceivedLabel = new JLabel("Number of messages received: "+nMessagesReceived));
        KafkaPanel.add(nMessagesInQueueLabel = new JLabel("Number of messages in queue: "+nMessagesInQueue));
        KafkaPanel.add(nMessagesFetchedLabel
                = new JLabel("Number of messages fetched by handler: "+nMessagesFetched));

        setKafkaInboundMessagesJList();
        jframe.add(KafkaPanel);
    }
    // Update list of Messages
    public static void setKafkaInboundMessagesJList(){
        kafkaInboundMessagesScrollPane = new JScrollPane();
        kafkaInboundMessagesJList
                = new JList<Message>(kafkaInboundMessagesList.toArray(new Message[kafkaInboundMessagesList.size()]));
        kafkaInboundMessagesScrollPane.setViewportView(kafkaInboundMessagesJList);
        kafkaInboundMessagesJList.setLayoutOrientation(JList.VERTICAL);
        KafkaPanel.add(kafkaInboundMessagesScrollPane);
    }
    // Update Kafka Panel
    public static void revalidateKafkaPanel(){
        nMessagesReceivedLabel.setText("Number of messages received: "+nMessagesReceived);
        nMessagesInQueueLabel.setText("Number of messages in queue: "+nMessagesInQueue);
        nMessagesFetchedLabel.setText("Number of messages fetched by handler: "+nMessagesFetched);
        Collections.sort(kafkaInboundMessagesList, new Comparator<Message>() {
            public int compare(Message o1, Message o2) {
                // compare two instance of `Score` and return `int` as result.
                try {
                    return (new Date(o1.Timestamp).compareTo(new Date(o2.Timestamp)));
                }catch(IllegalArgumentException e){
                    return 0;
                }

            }
        });
        kafkaInboundMessagesJList =
                new JList<Message>(kafkaInboundMessagesList.toArray(new Message[kafkaInboundMessagesList.size()]));
        kafkaInboundMessagesScrollPane.setViewportView(kafkaInboundMessagesJList);
        kafkaInboundMessagesJList.setLayoutOrientation(JList.VERTICAL);
        jframe.revalidate();
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // SocketHub Panel /////////////////////////////////////////////////////////////////////////////////////////////////
    public static int nHubConnections, nHubMessagesReceived, nHubMessagesInQueue, nHubMessagesBroadcasted;
    public static JLabel nHubMessagesReceivedLabel, nHubMessagesInQueueLabel, nHubMessagesBroadcastedLabel;
    public static ArrayList<Message> hubInboundMessagesList;
    public static JScrollPane hubInboundMessagesScrollPane, hubInboundConnectionRequestsScrollPane;
    public static JList<Message> hubInboundMessagesJList;
    public static ArrayList<String> hubInboundConnectionRequestList;
    public static JList<String> hubInboundConnectionRequestJList;

    public static void setSocketHubPanel(){
        SocketHubPanel = new JPanel();

        SocketHubPanel.setLayout(new BoxLayout(SocketHubPanel, BoxLayout.Y_AXIS));
        SocketHubPanel.setBorder(BorderFactory.createTitledBorder("Socket Hub"));

        SocketHubPanel.add(nHubMessagesReceivedLabel =
                new JLabel("Number of messages received: "+nHubMessagesReceived));
        SocketHubPanel.add(nHubMessagesInQueueLabel =
                new JLabel("Number of messages in queue: "+nHubMessagesInQueue));
        SocketHubPanel.add(nHubMessagesBroadcastedLabel =
                new JLabel("Number of messages broadcasted: "+nHubMessagesBroadcasted));

        setSocketHubInboundMessagesJList();
        setSocketHubInboundConnectionRequestJList();
        jframe.add(SocketHubPanel);
    }
    // Update list of Messages
    public static void setSocketHubInboundMessagesJList(){
        hubInboundMessagesScrollPane = new JScrollPane();
        hubInboundMessagesJList =
                new JList<Message>(hubInboundMessagesList.toArray(new Message[hubInboundMessagesList.size()]));
        hubInboundMessagesScrollPane.setViewportView(hubInboundMessagesJList);
        hubInboundMessagesJList.setLayoutOrientation(JList.VERTICAL);
        SocketHubPanel.add(hubInboundMessagesScrollPane);
    }
    // Update list of ConnectionRequest
    public static void setSocketHubInboundConnectionRequestJList(){
        hubInboundConnectionRequestsScrollPane = new JScrollPane();
        hubInboundConnectionRequestJList =
                new JList<String>(hubInboundConnectionRequestList.toArray(new String[hubInboundConnectionRequestList.size()]));
        hubInboundConnectionRequestsScrollPane.setViewportView(hubInboundConnectionRequestJList);
        hubInboundConnectionRequestJList.setLayoutOrientation(JList.VERTICAL);
        SocketHubPanel.add(hubInboundConnectionRequestsScrollPane);
    }
    // Update SocketHub Panel
    public static void revalidateSocketHubPanel(){
        nHubMessagesReceivedLabel.setText("Number of messages received: "+nHubMessagesReceived);
        nHubMessagesInQueueLabel.setText("Number of messages in queue: "+nHubMessagesInQueue);
        nHubMessagesBroadcastedLabel.setText("Number of messages broadcasted: "+nHubMessagesBroadcasted);
        Collections.sort(hubInboundMessagesList, new Comparator<Message>() {
            public int compare(Message o1, Message o2) {
                // compare two instance of `Score` and return `int` as result.
                return (new Date(o1.Timestamp).compareTo(new Date(o2.Timestamp)));
            }
        });
        hubInboundMessagesJList =
                new JList<Message>(hubInboundMessagesList.toArray(new Message[hubInboundMessagesList.size()]));
        hubInboundMessagesScrollPane.setViewportView(hubInboundMessagesJList);
        hubInboundMessagesJList.setLayoutOrientation(JList.VERTICAL);

        // ConnectioNRequest
        hubInboundConnectionRequestJList =
                new JList<String>(hubInboundConnectionRequestList.toArray(new String[hubInboundConnectionRequestList.size()]));
        hubInboundConnectionRequestsScrollPane.setViewportView(hubInboundConnectionRequestJList);
        hubInboundConnectionRequestJList.setLayoutOrientation(JList.VERTICAL);

        jframe.revalidate();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // SocketHub Panel /////////////////////////////////////////////////////////////////////////////////////////////////
    public static int nModuleMessagesReceived, nModuleMessagesInQueue, nModuleMessagesBroadcasted, nModuleMessagesDiscarded;
    public static JLabel nModuleMessagesReceivedLabel, nModuleMessagesInQueueLabel, nModuleMessagesBroadcastedLabel, nModuleMessagesDiscardedLabel;

    public static JScrollPane moduleInboundMessageScrollPane,moduleDiscardedMessageScrollPane, moduleBroadcastedMessageScrollPane;
    public static ArrayList<Message> moduleInboundMessageList, moduleDiscardedMessageList, moduleBroadcastedMessageList;
    public static JList<Message> moduleInboundMessageJList, moduleDiscardedMessageJList, moduleBroadcastedMessageJList;

    public static HashMap<ApplicationType, JScrollPane> connectionScrollPanel;
    public static HashMap<ApplicationType, ArrayList<String>> connectionsList;
    public static HashMap<ApplicationType, JList<String>> connectionsJList;

    public static void setModulesPanel(){
        ModulesPanel = new JPanel();

        ModulesPanel.setLayout(new BoxLayout(ModulesPanel, BoxLayout.Y_AXIS));
        ModulesPanel.setBorder(BorderFactory.createTitledBorder("Modules Hub"));

        ModulesJListPanel = new JPanel();
        ModulesJListPanel.setLayout(new BoxLayout(ModulesJListPanel, BoxLayout.X_AXIS));
        ModulesPanel.add(nModuleMessagesReceivedLabel =
                new JLabel("Number of messages received: "+nModuleMessagesReceived));
        ModulesPanel.add(nModuleMessagesInQueueLabel =
                new JLabel("Number of messages in queue: "+nModuleMessagesInQueue));
        ModulesPanel.add(nModuleMessagesBroadcastedLabel =
                new JLabel("Number of messages broadcasted: "+nModuleMessagesBroadcasted));
        ModulesPanel.add(nModuleMessagesDiscardedLabel =
                new JLabel("Number of messages discarded: "+nModuleMessagesDiscarded));
        setModulesInboundMessagesPanel();
        setModulesBroadcastedMessagesPanel();
        setModulesDiscardedMessagesPanel();
        setModulesConnectionsPanel();
        ModulesPanel.add(ModulesJListPanel);
        jframe.add(ModulesPanel);
    }
    // Update list of Messages
    public static void setModulesInboundMessagesPanel(){
        moduleInboundMessageScrollPane = new JScrollPane();
        moduleInboundMessageJList =
                new JList<Message>(moduleInboundMessageList.toArray(new Message[moduleInboundMessageList.size()]));
        moduleInboundMessageScrollPane.setViewportView(moduleInboundMessageJList);
        moduleInboundMessageJList.setLayoutOrientation(JList.VERTICAL);
        ModulesJListPanel.add(moduleInboundMessageScrollPane);
    }

    public static void setModulesBroadcastedMessagesPanel(){
        moduleBroadcastedMessageScrollPane = new JScrollPane();
        moduleBroadcastedMessageJList =
                new JList<Message>(moduleBroadcastedMessageList.toArray(new Message[moduleBroadcastedMessageList.size()]));
        moduleBroadcastedMessageScrollPane.setViewportView(moduleBroadcastedMessageJList);
        moduleBroadcastedMessageJList.setLayoutOrientation(JList.VERTICAL);
        ModulesJListPanel.add(moduleBroadcastedMessageScrollPane);
    }

    public static void setModulesDiscardedMessagesPanel(){
        moduleDiscardedMessageScrollPane = new JScrollPane();
        moduleDiscardedMessageJList =
                new JList<Message>(moduleDiscardedMessageList.toArray(new Message[moduleDiscardedMessageList.size()]));
        moduleDiscardedMessageScrollPane.setViewportView(moduleDiscardedMessageJList);
        moduleDiscardedMessageJList.setLayoutOrientation(JList.VERTICAL);
        ModulesJListPanel.add(moduleDiscardedMessageScrollPane);
    }

    public static void setModulesConnectionsPanel(){
        JPanel ConnectionsPanel = new JPanel(new GridLayout(2,2));
        for (ApplicationType type: ApplicationType.values()){
            if (type != ApplicationType.WEB_PLATFORM){
                connectionScrollPanel.put(type, new JScrollPane());
                connectionsJList.put(type,new JList<String>(connectionsList.get(type).toArray(new String[connectionsList.get(type).size()])));
                connectionScrollPanel.get(type).setViewportView(connectionsJList.get(type));
                connectionsJList.get(type).setLayoutOrientation(JList.VERTICAL);
                ConnectionsPanel.add(connectionScrollPanel.get(type));
            }
        }
        ModulesJListPanel.add(ConnectionsPanel);

    }
    // Update Modules Panel
    public static void revalidateModulesPanel(){
        nModuleMessagesReceivedLabel.setText("Number of messages received: "+nModuleMessagesReceived);
        nModuleMessagesInQueueLabel.setText("Number of messages in queue: "+nModuleMessagesInQueue);
        nModuleMessagesBroadcastedLabel.setText("Number of messages broadcasted: "+nModuleMessagesBroadcasted);
        nModuleMessagesDiscardedLabel.setText("Number of messages discarded: "+nModuleMessagesDiscarded);

        moduleInboundMessageJList = new JList<Message>(moduleInboundMessageList.toArray(new Message[moduleInboundMessageList.size()]));
        moduleInboundMessageScrollPane.setViewportView(moduleInboundMessageJList);
        moduleInboundMessageJList.setLayoutOrientation(JList.VERTICAL);

        moduleDiscardedMessageJList = new JList<Message>(moduleDiscardedMessageList.toArray(new Message[moduleDiscardedMessageList.size()]));
        moduleDiscardedMessageScrollPane.setViewportView(moduleDiscardedMessageJList);
        moduleDiscardedMessageJList.setLayoutOrientation(JList.VERTICAL);

        moduleBroadcastedMessageJList = new JList<Message>(moduleBroadcastedMessageList.toArray(new Message[moduleBroadcastedMessageList.size()]));
        moduleBroadcastedMessageScrollPane.setViewportView(moduleBroadcastedMessageJList);
        moduleBroadcastedMessageJList.setLayoutOrientation(JList.VERTICAL);

        for (ApplicationType type: ApplicationType.values()) {
            if (type != ApplicationType.WEB_PLATFORM) {
                connectionsJList.put(type, new JList<String>(connectionsList.get(type).toArray(new String[connectionsList.get(type).size()])));
                connectionScrollPanel.get(type).setViewportView(connectionsJList.get(type));
                connectionsJList.get(type).setLayoutOrientation(JList.VERTICAL);
            }
        }
        jframe.revalidate();
    }
}
