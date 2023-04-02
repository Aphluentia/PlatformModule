package kafka.guis;

import kafka.App;
import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.ServerConfig;

import javax.swing.*;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import java.awt.*;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.awt.event.WindowEvent;
import java.util.HashMap;

import static javax.swing.JOptionPane.showMessageDialog;

public class ConfigPrompt {
    public static JTextField brokerConnection, kafkaTopic, logsFile;
    public static HashMap<ApplicationType, JTextField> socketPorts;
    public static JTextField socketHubPort;

    public static void generateConfigurationGui(){
        JFrame jframe = new JFrame("Configuration Screen");
        jframe.getContentPane().setLayout(new BoxLayout(jframe.getContentPane(), BoxLayout.Y_AXIS));

        //Config Panel
        JPanel mainPanel = new JPanel(new GridLayout(1,2));

        JPanel mainConfigPanel = new JPanel();
        mainConfigPanel.setLayout(new BoxLayout(mainConfigPanel, BoxLayout.Y_AXIS));
        mainConfigPanel.setBorder(BorderFactory.createTitledBorder("Main Configs"));
        mainConfigPanel = ConfigPrompt.setUpMainConfigs(mainConfigPanel);
        mainConfigPanel.setMaximumSize(new Dimension(400,600));
        mainPanel.add(mainConfigPanel, BorderLayout.WEST);



        JPanel socketsPanel = new JPanel();
        socketsPanel.setLayout(new BoxLayout(socketsPanel, BoxLayout.Y_AXIS));
        socketsPanel.setBorder(BorderFactory.createTitledBorder("Sockets Configs"));
        socketsPanel = ConfigPrompt.setUpSocketConfigs(socketsPanel);
        socketsPanel.setMaximumSize(new Dimension(400,600));
        mainPanel.add(socketsPanel, BorderLayout.EAST);

        //Button Panel
        JPanel buttonPanel = new JPanel();
        buttonPanel.setLayout(new BoxLayout(buttonPanel, BoxLayout.X_AXIS));
        buttonPanel.setPreferredSize(new Dimension(600, 100));
        JButton firstButton = new JButton("Start Server");
        firstButton.addActionListener(new ActionListener() {
            @Override
            public void actionPerformed(ActionEvent e) {
                if (Validate()){
                    App.startApp();
                    jframe.dispatchEvent(new WindowEvent(jframe, WindowEvent.WINDOW_CLOSING));
                }
            }
        });
        buttonPanel.add(firstButton, BorderLayout.SOUTH);
        jframe.add(mainPanel, BorderLayout.PAGE_START);
        jframe.add(buttonPanel, BorderLayout.SOUTH);

        jframe.setSize(600, 600);
        jframe.setVisible(true);
    }
    public static JPanel setUpMainConfigs(JPanel mainConfigsPanel) {
        JPanel threadConfigs = new JPanel();
        threadConfigs.setLayout(new BoxLayout(threadConfigs, BoxLayout.Y_AXIS));
        threadConfigs.setBorder(BorderFactory.createTitledBorder("Thread Configs"));
        // No Kafka Consumer Threads
        JSlider nKCSlider = new JSlider(JSlider.HORIZONTAL, 1, 5, 1);
        nKCSlider.setBorder(BorderFactory.createTitledBorder("Number of Kafka Consumers"));
        nKCSlider.setPaintTicks(true);
        nKCSlider.setPaintLabels(true);
        nKCSlider.setMinorTickSpacing(1);
        nKCSlider.setMajorTickSpacing(1);
        nKCSlider.addChangeListener(new ChangeListener() {
            public void stateChanged(ChangeEvent e) {
                ServerConfig.NO_KAFKA_CONSUMERS = nKCSlider.getValue();
            }
        });
        threadConfigs.add(nKCSlider);


        JSlider nMHSlider = new JSlider(JSlider.HORIZONTAL, 1, 5, 1);
        nMHSlider.setPaintTicks(true);
        nMHSlider.setPaintLabels(true);
        nMHSlider.setMinorTickSpacing(1);
        nMHSlider.setMajorTickSpacing(1);
        nMHSlider.addChangeListener(new ChangeListener() {
            public void stateChanged(ChangeEvent e) {
                ServerConfig.NO_MESSAGE_HANDLERS = nMHSlider.getValue();
            }
        });
        nMHSlider.setBorder(BorderFactory.createTitledBorder("Number of Message Handlers"));
        threadConfigs.add(nMHSlider);

        JSlider nMBSlider = new JSlider(JSlider.HORIZONTAL, 1, 5, 1);
        nMBSlider.setPaintTicks(true);
        nMBSlider.setPaintLabels(true);
        nMBSlider.setMinorTickSpacing(1);
        nMBSlider.setMajorTickSpacing(1);
        nMBSlider.addChangeListener(new ChangeListener() {
            public void stateChanged(ChangeEvent e) {
                ServerConfig.NO_MODULES_BROADCASTERS = nMBSlider.getValue();
            }
        });
        nMBSlider.setBorder(BorderFactory.createTitledBorder("Number of Message Broadcasters"));
        threadConfigs.add(nMBSlider);
        mainConfigsPanel.add(threadConfigs);

        // Kafka Configs
        JPanel kafkaConfigs = new JPanel(new GridLayout(2,1));
        kafkaConfigs.setBorder(BorderFactory.createTitledBorder("Kafka Configurations"));

        JPanel brokerConnectionPanel = new JPanel();
        brokerConnectionPanel.setLayout(new BoxLayout(brokerConnectionPanel, BoxLayout.Y_AXIS));
        brokerConnectionPanel.setBorder(BorderFactory.createTitledBorder(" Port"));
        ConfigPrompt.brokerConnection = new JTextField(ServerConfig.BOOTSTRAP_SERVERS);
        ConfigPrompt.brokerConnection.setMaximumSize(new Dimension(400, 30));
        brokerConnectionPanel.add(ConfigPrompt.brokerConnection);
        kafkaConfigs.add(brokerConnectionPanel);

        JPanel kafkaTopicPanel = new JPanel();
        kafkaTopicPanel.setLayout(new BoxLayout(kafkaTopicPanel, BoxLayout.Y_AXIS));
        kafkaTopicPanel.setBorder(BorderFactory.createTitledBorder("Kafka Broker Topic"));
        ConfigPrompt.kafkaTopic = new JTextField(ServerConfig.TOPIC);
        ConfigPrompt.kafkaTopic.setMaximumSize(new Dimension(400, 30));
        kafkaTopicPanel.add(ConfigPrompt.kafkaTopic);
        kafkaConfigs.add(kafkaTopicPanel);

        mainConfigsPanel.add(kafkaConfigs);
        // Logging Configs Panel
        JPanel logsConfig = new JPanel();
        logsConfig.setLayout(new BoxLayout(logsConfig, BoxLayout.Y_AXIS));
        logsConfig.setBorder(BorderFactory.createTitledBorder("Logging Config"));


        JPanel logFilePanel = new JPanel();
        logFilePanel.setLayout(new BoxLayout(logFilePanel, BoxLayout.Y_AXIS));
        logFilePanel.setBorder(BorderFactory.createTitledBorder("Logging File Path"));
        ConfigPrompt.logsFile  = new JTextField(ServerConfig.LOGS_FILEPATH);
        ConfigPrompt.logsFile.setMaximumSize(new Dimension(400, 30));
        logFilePanel.add(ConfigPrompt.logsFile);
        logsConfig.add(logFilePanel);

        mainConfigsPanel.add(logsConfig);
        return mainConfigsPanel;
    }
    public static JPanel setUpSocketConfigs(JPanel socketsPanel){
        JPanel socketHubPanel  = new JPanel();
        socketHubPanel.setLayout(new BoxLayout(socketHubPanel, BoxLayout.Y_AXIS));
        socketHubPanel.setMaximumSize(new Dimension(400, 50));
        socketHubPanel.setBorder(BorderFactory.createTitledBorder("Socket Hub Port"));
        ConfigPrompt.socketHubPort  = new JTextField(Integer.toString(ServerConfig.SOCKET_HUB_PORT),4);
        socketHubPanel.add(ConfigPrompt.socketHubPort);
        socketsPanel.add(socketHubPanel);

        // Socket Server Ports
        ConfigPrompt.socketPorts = new HashMap<>();
        JPanel serverSocketPanel = new JPanel();
        serverSocketPanel.setLayout(new BoxLayout(serverSocketPanel, BoxLayout.Y_AXIS));
        serverSocketPanel.setBorder(BorderFactory.createTitledBorder("AppType Socket Ports"));
        for (ApplicationType appType: ApplicationType.values()){
            JPanel socket = new JPanel();
            socket.setLayout(new BoxLayout(socket, BoxLayout.Y_AXIS));
            socket.setBorder(BorderFactory.createTitledBorder(appType + " Port"));
            socket.setMaximumSize(new Dimension(400, 50));
            JTextField socketTextField = new JTextField(Integer.toString(ServerConfig.MODULES_PORT.get(appType)), 4);
            ConfigPrompt.socketPorts.put(appType, socketTextField);
            socket.add(socketTextField);
            serverSocketPanel.add(socket);
        }
        socketsPanel.add(serverSocketPanel);
        return socketsPanel;
    }

    public static boolean Validate(){
        boolean valid = true;
        ServerConfig.BOOTSTRAP_SERVERS = ConfigPrompt.brokerConnection.getText();
        ServerConfig.TOPIC = ConfigPrompt.kafkaTopic.getText();
        ServerConfig.LOGS_FILEPATH = ConfigPrompt.logsFile.getText();

        for (ApplicationType app: ApplicationType.values()) {
            try {
                int port = Integer.parseInt(ConfigPrompt.socketPorts.get(app).getText());
                ServerConfig.MODULES_PORT.put(app, port);
            }catch (Exception e){
                valid = false;
            }
        }
        try {
            int port = Integer.parseInt(ConfigPrompt.socketHubPort.getText());
            ServerConfig.SOCKET_HUB_PORT = port;
        }catch (Exception e){
            valid = false;
        }
        return valid;
    }
}
