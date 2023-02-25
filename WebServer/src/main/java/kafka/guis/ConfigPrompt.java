package kafka.guis;

import kafka.Entities.Enum.AppType;
import kafka.Entities.Enum.ServerConfig;

import javax.swing.*;
import javax.swing.event.ChangeEvent;
import javax.swing.event.ChangeListener;
import java.awt.*;

public class ConfigPrompt {
    public static JTextField brokerConnection, kafkaTopic, logsFile;

    public static void generateConfigurationGui(){


        JFrame jframe = new JFrame("Configuration Screen");
        jframe.getContentPane().setLayout(new BoxLayout(jframe.getContentPane(), BoxLayout.Y_AXIS));

        //Config Panel
        JPanel mainPanel = new JPanel();
        mainPanel.setLayout(new BoxLayout(mainPanel, BoxLayout.X_AXIS));
        mainPanel.setPreferredSize(new Dimension(600, 500));
        JPanel mainConfigPanel = new JPanel( new GridLayout(3, 1));
        mainConfigPanel.setBorder(BorderFactory.createTitledBorder("Main Configs"));
        mainConfigPanel = ConfigPrompt.setUpMainConfigs(mainConfigPanel);
        mainPanel.add(mainConfigPanel, BorderLayout.WEST);



        JPanel socketsPanel = new JPanel(new GridLayout(AppType.values().length+1, 1));
        socketsPanel.setBorder(BorderFactory.createTitledBorder("Sockets Configs"));
        mainPanel.add(socketsPanel, BorderLayout.EAST);

        //Button Panel
        JPanel buttonPanel = new JPanel();
        buttonPanel.setLayout(new BoxLayout(buttonPanel, BoxLayout.X_AXIS));
        buttonPanel.setPreferredSize(new Dimension(600, 100));
        JButton firstButton = new JButton("Start Server");
        buttonPanel.add(firstButton, BorderLayout.SOUTH);

        jframe.add(mainPanel, BorderLayout.PAGE_START);
        jframe.add(buttonPanel, BorderLayout.SOUTH);
        jframe.setSize(600, 600);

        jframe.pack();
        jframe.setVisible(true);
    }
    public static JPanel setUpMainConfigs(JPanel mainConfigsPanel) {
        JPanel threadConfigs = new JPanel(new GridLayout(2, 1));
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
        mainConfigsPanel.add(threadConfigs);

        // Kafka Configs

        JPanel kafkaConfigs = new JPanel(new GridLayout(2, 1));
        kafkaConfigs.setBorder(BorderFactory.createTitledBorder("Kafka Configs"));

        ConfigPrompt.brokerConnection = new JTextField(ServerConfig.BOOTSTRAP_SERVERS);
        ConfigPrompt.brokerConnection.setBorder(BorderFactory.createTitledBorder("Kafka Broker Connection String"));
        kafkaConfigs.add(ConfigPrompt.brokerConnection);

        ConfigPrompt.kafkaTopic  = new JTextField(ServerConfig.TOPIC);
        ConfigPrompt.kafkaTopic.setBorder(BorderFactory.createTitledBorder("Kafka Broker Topic"));
        kafkaConfigs.add(ConfigPrompt.kafkaTopic);
        mainConfigsPanel.add(kafkaConfigs);

        JPanel logsConfig = new JPanel();
        logsConfig.setLayout(new GridLayout(2,1));
        logsConfig.setBorder(BorderFactory.createTitledBorder("Logging Config"));
        ConfigPrompt.logsFile  = new JTextField(ServerConfig.LOGS_FILEPATH);
        ConfigPrompt.logsFile.setBorder(BorderFactory.createTitledBorder("Logging File"));
        logsConfig.add(ConfigPrompt.logsFile);
        mainConfigsPanel.add(logsConfig);
        return mainConfigsPanel;
    }
    public static JPanel setUpSocketConfigs(JPanel socketsPanel){
        return socketsPanel;
    }
}
