package kafka;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.ServerConfig;
import kafka.Entities.Interfaces.SocketHubMonitor.IHubBroadcaster;
import kafka.Entities.Threads.*;
import kafka.Monitors.*;
import kafka.guis.ConfigPrompt;
import kafka.guis.TServerGui;
import org.apache.kafka.clients.consumer.ConsumerConfig;
import org.apache.kafka.common.serialization.StringDeserializer;

import java.util.Properties;


public class App 
{
    public static void main( String[] args )
    {
        ServerConfig.DefaultConfig();
        ConfigPrompt.generateConfigurationGui();

    }
    public static void startApp(){
        MGui mGui = new MGui();
        MLogger mlogger = new MLogger();
        MKafka mKafka = new MKafka(mlogger);
        MModules mModules = new MModules(mlogger);
        MSocketHub mSocketHub  = new MSocketHub(mlogger);
        // Consumer Properties
        final Properties props = new Properties();
        props.put(ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG, ServerConfig.BOOTSTRAP_SERVERS);
        props.put(ConsumerConfig.KEY_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.put(ConsumerConfig.VALUE_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.setProperty(ConsumerConfig.AUTO_OFFSET_RESET_CONFIG, "latest");

        TServerGui tsgui = new TServerGui();
        tsgui.start();
        //Socket Definition
        for (ApplicationType appType: ApplicationType.values()){
            Thread _tkc = new TModulesSocketServer(mModules, mGui, appType, ServerConfig.MODULES_PORT.get(appType), mlogger);
            _tkc.start();
        }
        Thread tLogger = new TLogger(mlogger, ServerConfig.LOGS_FILEPATH);
        tLogger.start();
        // Start Web Socket Hub
        Thread webSocketHub = new TSocketHub(ServerConfig.MODULES_PORT, ServerConfig.SOCKET_HUB_PORT,mSocketHub,mGui, mlogger);
        webSocketHub.start();


        // Start Kafka Message Handlers
        for (int i = 0;i<ServerConfig.NO_MESSAGE_HANDLERS;i++){
            Thread _tkc = new TKafkaMessageHandler(mKafka, mModules,mSocketHub, mGui, mlogger);
            _tkc.start();
        }
        // Start Modules Broadcasters
        for (int i = 0;i<ServerConfig.NO_MODULES_BROADCASTERS;i++){
            Thread _tkc = new TModulesBroadcaster(mModules, mGui, mlogger);
            _tkc.start();
        }
        for (int i = 0;i<ServerConfig.NO_MODULES_BROADCASTERS;i++){
            Thread _tkc = new TSocketHubBroadcaster(mSocketHub, mGui, ServerConfig.MODULES_PORT, mlogger);
            _tkc.start();
        }
        for (int i = 0;i<ServerConfig.NO_MESSAGE_HANDLERS;i++){
            Thread _tkc = new TModuleRejectedMessageHandler(mModules, mGui, mlogger);
            _tkc.start();
        }
        // Start Kafka Consumers
        for (int i = 0;i<ServerConfig.NO_KAFKA_CONSUMERS;i++){
            props.put(ConsumerConfig.GROUP_ID_CONFIG,"KafkaConsumerGroup#"+ i);
            Thread _tkc = new TKafkaConsumer(mKafka,mGui, props, mlogger);
            _tkc.start();
        }


    }
}
