package kafka;

import kafka.Entities.*;
import kafka.Entities.Enum.AppType;
import kafka.Entities.Threads.*;
import kafka.Monitors.MKafka;
import kafka.Monitors.MLogger;
import kafka.Monitors.MWebSockets;
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
        MLogger mlogger = new MLogger();
        MKafka mKafka = new MKafka(mlogger);
        MWebSockets mWebSockets = new MWebSockets(mlogger);

        final Properties props = new Properties();
        props.put(ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG, Constants.BOOTSTRAP_SERVERS);
        props.put(ConsumerConfig.KEY_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.put(ConsumerConfig.VALUE_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.setProperty(ConsumerConfig.AUTO_OFFSET_RESET_CONFIG, "latest");

        //Socket Definition
        HashMap<AppType, Integer> _typePorts = new HashMap<>();
        int _port = Constants.BASE_MODULES_PORT;
        for (AppType appType: AppType.values()){
            Thread _tkc = new TSocketServer(mWebSockets, appType, _port, mlogger);
            _tkc.start();
            _typePorts.put(appType, _port);
            _port++;
        }
        Thread tLogger = new TLogger(mlogger, Constants.FILEPATH);
        tLogger.start();
        Thread wshub = new TWebSocketHub(_typePorts, Constants.SOCKET_HUB_PORT, mlogger);
        wshub.start();
        // Threads For Monitors
        for (int i = 0;i<Constants.NO_MESSAGE_HANDLERS;i++){
            Thread _tkc = new TMessageHandler(mKafka, mWebSockets, mlogger);
            _tkc.start();
        }
        for (int i = 0;i<Constants.NO_KAFKA_CONSUMERS;i++){
            props.put(ConsumerConfig.GROUP_ID_CONFIG,"KafkaConsumerGroup#"+ i);
            Thread _tkc = new TKafkaConsumer(mKafka, props, mlogger);
            _tkc.start();
        }

    }
}
