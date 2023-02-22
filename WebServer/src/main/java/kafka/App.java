package kafka;

import kafka.Entities.*;
import kafka.Entities.Enum.AppType;
import kafka.Entities.Threads.TKafkaConsumer;
import kafka.Entities.Threads.TMessageHandler;
import kafka.Entities.Threads.TSocketServer;
import kafka.Entities.Threads.TWebSocketHub;
import kafka.Monitors.MKafka;
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
        System.out.println( "Hello World!" );
        MKafka mKafka = new MKafka();
        MWebSockets mWebSockets = new MWebSockets();

        final Properties props = new Properties();
        props.put(ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG, Constants.BOOTSTRAP_SERVERS);
        props.put(ConsumerConfig.KEY_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.put(ConsumerConfig.VALUE_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.setProperty(ConsumerConfig.AUTO_OFFSET_RESET_CONFIG, "latest");

        //Socket Definition
        HashMap<AppType, Integer> _typePorts = new HashMap<>();
        int _port = Constants.BASE_MODULES_PORT;
        for (AppType appType: AppType.values()){
            Thread _tkc = new TSocketServer((ISocketConnectionHandler) mWebSockets, appType, _port);
            _tkc.start();
            _typePorts.put(appType, _port);
            _port++;
        }
        Thread wshub = new TWebSocketHub(_typePorts, Constants.SOCKET_HUB_PORT);
        wshub.start();
        // Threads For Monitors
        for (int i = 0;i<Constants.NO_MESSAGE_HANDLERS;i++){
            Thread _tkc = new TMessageHandler((IMessageHandler) mKafka, (ISocketMessageHandler)mWebSockets);
            _tkc.start();
        }
        for (int i = 0;i<Constants.NO_KAFKA_CONSUMERS;i++){
            props.put(ConsumerConfig.GROUP_ID_CONFIG,"KafkaConsumerGroup#"+String.valueOf(i));
            Thread _tkc = new TKafkaConsumer((IKafkaConsumer) mKafka, props);
            _tkc.start();
        }




    }
}
