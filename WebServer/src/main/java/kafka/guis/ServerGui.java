package kafka.guis;

import kafka.Entities.Enum.ApplicationType;

import javax.swing.*;
import java.awt.*;

public class ServerGui {
    JFrame jframe;
    JPanel messagesPanel, socketHubPanel, appTypePanel;

    //Panels: SocketHubPanel, (KafkaPanel, MessageHandler), AppTypePanel
    public ServerGui(){
        jframe = new JFrame("Web Server");
        jframe.getContentPane().setLayout(new BoxLayout(jframe.getContentPane(), BoxLayout.Y_AXIS));
        init();
        //generateMessagesPanel();    //(KafkaPanel, MessageHandler)
        //generateSocketHubPanel();
        //generateAppTypePanel();

    }
    public void init(){
        // Contains Messages Received, Messages in Monitor, Messages Handled
        this.messagesPanel = new JPanel(new GridLayout(3,1));
        // Contains Current Connections, CurrentConnectionsAssigned, Connections per type, list of connections
        this.socketHubPanel = new JPanel(new GridLayout(3,1));
        this.appTypePanel = new JPanel(new GridLayout(ApplicationType.values().length, 1));

    }
    public void updateSocketHub(){
        jframe.revalidate();
    }
}
