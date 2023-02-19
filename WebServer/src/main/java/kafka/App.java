package kafka;

import kafka.Entities.IKafkaConsumer;
import kafka.Entities.IMessageHandler;
import kafka.Entities.TKafkaConsumer;
import kafka.Entities.TMessageHandler;
import kafka.Monitors.MKafka;
import org.apache.kafka.clients.consumer.ConsumerConfig;
import org.apache.kafka.common.serialization.StringDeserializer;

import java.util.Properties;
import java.util.UUID;

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

        final Properties props = new Properties();
        props.put(ConsumerConfig.BOOTSTRAP_SERVERS_CONFIG, Constants.BOOTSTRAP_SERVERS);
        props.put(ConsumerConfig.KEY_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.put(ConsumerConfig.VALUE_DESERIALIZER_CLASS_CONFIG, StringDeserializer.class.getName());
        props.setProperty(ConsumerConfig.AUTO_OFFSET_RESET_CONFIG, "earliest");
        for (int i = 0;i<Constants.NO_MESSAGE_HANDLERS;i++){
            Thread _tkc = new TMessageHandler((IMessageHandler) mKafka);
            _tkc.start();
        }
        for (int i = 0;i<Constants.NO_KAFKA_CONSUMERS;i++){
            props.put(ConsumerConfig.GROUP_ID_CONFIG,"KafkaConsumerGroup#"+String.valueOf(i));
            Thread _tkc = new TKafkaConsumer((IKafkaConsumer) mKafka, props);
            _tkc.start();
        }




    }
}
