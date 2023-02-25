package kafka;

import kafka.Entities.*;
import kafka.Entities.Enum.AppType;
import kafka.Entities.Enum.ServerConfig;
import kafka.Entities.Threads.*;
import kafka.Monitors.MKafka;
import kafka.Monitors.MLogger;
import kafka.Monitors.MWebSockets;
import kafka.guis.ConfigPrompt;
import org.apache.kafka.clients.consumer.ConsumerConfig;
import org.apache.kafka.common.serialization.StringDeserializer;

import java.util.HashMap;
import java.util.Properties;

/**
 * Hello world!
 *
 */
public class App 
{
    public static void main( String[] args )
    {
        ServerConfig.DefaultConfig();
        ConfigPrompt.generateConfigurationGui();
        MLogger mlogger = new MLogger();
        MKafka mKafka = new MKafka(mlogger);
        MWebSockets mWebSockets = new MWebSockets(mlogger);

        final Properties props = new Properties();
        props.put(ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG, ServerConfig.BOOTSTRAP_SERVERS);
        props.put(ConsumerConfig.KEY_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.put(ConsumerConfig.VALUE_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.setProperty(ConsumerConfig.AUTO_OFFSET_RESET_CONFIG, "latest");

        //Socket Definition
        for (AppType appType: AppType.values()){
            Thread _tkc = new TSocketServer(mWebSockets, appType, ServerConfig.MODULES_PORT.get(appType), mlogger);
            _tkc.start();
        }
        Thread tLogger = new TLogger(mlogger, ServerConfig.LOGS_FILEPATH);
        tLogger.start();
        Thread wshub = new TWebSocketHub(ServerConfig.MODULES_PORT, ServerConfig.SOCKET_HUB_PORT, mlogger);
        wshub.start();
        // Threads For Monitors
        for (int i = 0;i<ServerConfig.NO_MESSAGE_HANDLERS;i++){
            Thread _tkc = new TMessageHandler(mKafka, mWebSockets, mlogger);
            _tkc.start();
        }
        for (int i = 0;i<ServerConfig.NO_KAFKA_CONSUMERS;i++){
            props.put(ConsumerConfig.GROUP_ID_CONFIG,"KafkaConsumerGroup#"+ i);
            Thread _tkc = new TKafkaConsumer(mKafka, props, mlogger);
            _tkc.start();
        }
    }
}
