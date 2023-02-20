package kafka;

import kafka.Entities.*;
import kafka.Entities.Threads.TKafkaConsumer;
import kafka.Entities.Threads.TMessageHandler;
import kafka.Entities.Threads.TWebSocketHub;
import kafka.Monitors.MKafka;
import kafka.Monitors.MWebSockets;
import org.apache.kafka.clients.consumer.ConsumerConfig;
import org.apache.kafka.common.serialization.StringDeserializer;

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
        int port = 9005;
        for (AppType appType: AppType.values()){
            Thread _tkc = new TWebSocketHub((ISocketConnectionHandler) mWebSockets, appType,port);
            port++;
            _tkc.start();
        }
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
