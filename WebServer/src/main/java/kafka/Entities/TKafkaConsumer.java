package kafka.Entities;

import org.apache.kafka.clients.consumer.Consumer;
import org.apache.kafka.clients.consumer.ConsumerRecords;
import org.apache.kafka.clients.consumer.KafkaConsumer;

import java.time.Duration;
import java.util.Collections;
import java.util.Properties;
import kafka.Constants;

public class TKafkaConsumer extends Thread{
    /**
     * All Monitor Call Center Interfaces -> Includes CCH, ETH, WTH and MDH
     */
    private final IKafkaConsumer ikc;
    private Consumer<String, String> consumer;
    /**
     * Boolean flag for suspending process
     */
    private boolean threadSuspended;

    /**
     * Boolean Flag for stopping process
     */
    private boolean stopFlag;

    /**
     * <b>Class Constructor</b>
     * <p>threadSuspended and stopFlag initialized as False</p>
     * @param _ikc: Interface  for the MKafka Monitor
     */
    public TKafkaConsumer(IKafkaConsumer _ikc, Properties _props) {
        this.ikc = _ikc;
        this.threadSuspended = false;
        this.stopFlag = false;
        this.consumer = new KafkaConsumer<String, String>(_props);

    }

    /**
     * <p>threadSuspended flag set to true, CCH waits for it to be false again</p>
     */
    public synchronized void suspendProcess(){
        this.threadSuspended = true;
    }
    /**
     * <p>threadSuspended flag set to false, suspended call centre is notified and resumes process</p>
     */
    public synchronized void resumeProcess(){
        this.threadSuspended = false;
        notify();
    }

    /**
     * <p>stopFlag flag set to true, process ends</p>
     */
    public void stopProcess() {
        this.stopFlag = true;
    }

    /**
     * <p>Run thread method</p>
     *<p>
     * While the process is running, the Call Centre keeps the simulation
     * running by receiving new SIGNAl signals.
     * If the result of the action produced by the received signal is unsatisfatory,
     * this last SIGNAL signal is added to the end of the message list in the CCH
     * </p>
     * <p> Unknown Signal launches an Error</p>
     */
    @Override
    public void run() {
        consumer.subscribe(Collections.singletonList(Constants.TOPIC));
        try {
            while(!this.stopFlag){
                synchronized(this) {
                    while (threadSuspended) {
                        try {
                            wait();
                        } catch (InterruptedException e) {
                            e.printStackTrace();
                        }
                    }
                }

                while (true) {
                    ConsumerRecords<String, String> records =
                            consumer.poll(Duration.ofMillis(100));
                    records.forEach(record -> {
                        this.ikc.addMessage(record.value());
                    });
                }
            }

        } catch (Exception e) {
            System.out.println(e);
        }
    }
}