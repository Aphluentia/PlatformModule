package kafka.Entities.Threads;

import kafka.Constants;
import kafka.Entities.AppType;
import kafka.Entities.IMessageHandler;
import kafka.Entities.ISocketMessageHandler;
import kafka.Entities.Message;
import org.apache.kafka.clients.consumer.Consumer;
import org.apache.kafka.clients.consumer.ConsumerRecords;
import org.apache.kafka.clients.consumer.KafkaConsumer;

import java.util.Collections;
import java.util.Properties;

public class TMessageHandler extends Thread{
    /**
     * All Monitor Call Center Interfaces -> Includes CCH, ETH, WTH and MDH
     */
    private final IMessageHandler imc;
    private final ISocketMessageHandler ismh;
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
     * @param _imc: Interface  for the MKafka Monitor
     */
    public TMessageHandler(IMessageHandler _imc, ISocketMessageHandler _ismh) {
        this.imc = _imc;
        this.ismh = _ismh;
        this.threadSuspended = false;
        this.stopFlag = false;
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
                    Message str = this.imc.handleMessage();
                    System.out.println("TMessageHandler "+ str.toString());
                    this.ismh.addMessage(str, Enum.valueOf(AppType.class, str.ApplicationType));

                }
            }

        } catch (Exception e) {
            System.out.println(e);
        }
    }
}
