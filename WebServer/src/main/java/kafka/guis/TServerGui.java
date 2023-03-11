package kafka.guis;

import kafka.Entities.Models.Message;

import javax.swing.*;
import java.awt.*;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.Date;

public class TServerGui extends Thread{
    public static JFrame jframe;
    public static JPanel KafkaPanel, socketHubPanel, appTypePanel;

    // KafkaPanel
    public static int nMessagesReceived, nMessagesInQueue, nMessagesFetched;
    public static JLabel nMessagesReceivedLabel, nMessagesInQueueLabel, nMessagesFetchedLabel;
    public static ArrayList<Message> kafkaInboundMessagesList;
    public static JScrollPane kafkaInboundMessagesScrollPane;
    public static JList<Message> kafkaInboundMessagesJList;
    //Panels: SocketHubPanel, (KafkaPanel, MessageHandler), AppTypePanel
    public TServerGui(){
        TServerGui.jframe = new JFrame("Web Server");
        kafkaInboundMessagesList = new ArrayList<>();
        TServerGui.jframe.getContentPane().setLayout(new BoxLayout(jframe.getContentPane(), BoxLayout.Y_AXIS));
        setKafkaPanel();
        //generateMessagesPanel();    //(KafkaPanel, MessageHandler)
        //generateSocketHubPanel();
        //generateAppTypePanel();
        TServerGui.jframe.setSize(1080,1920);
        TServerGui.jframe.setVisible(true);

    }
    public static void setKafkaPanel(){
        KafkaPanel = new JPanel();

        KafkaPanel.setLayout(new BoxLayout(KafkaPanel, BoxLayout.Y_AXIS));
        KafkaPanel.setMaximumSize(new Dimension(600,300));
        KafkaPanel.setBorder(BorderFactory.createTitledBorder("Kafka Monitor"));

        KafkaPanel.add(nMessagesReceivedLabel = new JLabel("Number of messages received: "+nMessagesReceived));
        KafkaPanel.add(nMessagesInQueueLabel = new JLabel("Number of messages in queue: "+nMessagesInQueue));
        KafkaPanel.add(nMessagesFetchedLabel = new JLabel("Number of messages fetched by handler: "+nMessagesFetched));

        setKafkaInboundMessagesJList();
        jframe.add(KafkaPanel);
    }
    public static void setKafkaInboundMessagesJList(){
        kafkaInboundMessagesScrollPane = new JScrollPane();
        kafkaInboundMessagesJList = new JList<Message>(kafkaInboundMessagesList.toArray(new Message[kafkaInboundMessagesList.size()]));
        kafkaInboundMessagesScrollPane.setViewportView(kafkaInboundMessagesJList);
        kafkaInboundMessagesJList.setLayoutOrientation(JList.VERTICAL);
        KafkaPanel.add(kafkaInboundMessagesScrollPane);
    }




    public static void revalidateKafkaPanel(){

        nMessagesReceivedLabel.setText("Number of messages received: "+nMessagesReceived);
        nMessagesInQueueLabel.setText("Number of messages in queue: "+nMessagesInQueue);
        nMessagesFetchedLabel.setText("Number of messages fetched by handler: "+nMessagesFetched);
        Collections.sort(kafkaInboundMessagesList, new Comparator<Message>() {
            public int compare(Message o1, Message o2) {
                // compare two instance of `Score` and return `int` as result.
                return (new Date(o1.Timestamp).compareTo(new Date(o2.Timestamp)));
            }
        });
        kafkaInboundMessagesJList = new JList<Message>(kafkaInboundMessagesList.toArray(new Message[kafkaInboundMessagesList.size()]));
        kafkaInboundMessagesScrollPane.setViewportView(kafkaInboundMessagesJList);
        kafkaInboundMessagesJList.setLayoutOrientation(JList.VERTICAL);

        System.out.println(nMessagesReceived);
        jframe.revalidate();
    }
}
