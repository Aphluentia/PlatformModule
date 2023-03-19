package kafka.Entities.Threads;

import com.google.gson.Gson;
import kafka.Entities.Enum.*;
import kafka.Entities.Interfaces.GuiMonitor.IGui;
import kafka.Entities.Interfaces.KafkaMonitor.IKafkaConsumer;
import kafka.Entities.Models.Message;
import kafka.Entities.Models.ServerLog;
import kafka.Monitors.MLogger;
import kafka.guis.TServerGui;
import org.apache.kafka.clients.consumer.Consumer;
import org.apache.kafka.clients.consumer.ConsumerRecords;
import org.apache.kafka.clients.consumer.KafkaConsumer;

import java.time.Duration;
import java.util.Arrays;
import java.util.Date;
import java.util.Properties;

// NO CHANGES---------------------------------------------------------------------
public class TKafkaConsumer extends Thread{
    /**
     * All Monitor Call Center Interfaces -> Includes CCH, ETH, WTH and MDH
     */
    private final IKafkaConsumer ikc;
    private final Consumer<String, String> consumer;

    private boolean threadSuspended;
    private boolean stopFlag;
    private final MLogger mlogger;
    private final IGui gui;
    /**
     * <b>Class Constructor</b>
     * <p>threadSuspended and stopFlag initialized as False</p>
     * @param _ikc: Interface  for the MKafka Monitor
     */
    public TKafkaConsumer(IKafkaConsumer _ikc, IGui _gui, Properties _props, MLogger _mlogger) {
        this.mlogger =_mlogger;
        this.ikc = _ikc;
        this.gui = _gui;
        this.threadSuspended = false;
        this.stopFlag = false;
        this.consumer = new KafkaConsumer<>(_props);
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
     * If the result of the action produced by the received signal is unsatisfactory,
     * this last SIGNAL signal is added to the end of the message list in the CCH
     * </p>
     * <p> Unknown Signal launches an Error</p>
     */
    @Override
    public void run() {
        consumer.subscribe(Arrays.asList(ServerConfig.TOPIC.split(",")));
        this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TKafkaConsumer Broker %s Connection on Topic %s ", ServerConfig.BOOTSTRAP_SERVERS, ServerConfig.TOPIC)));
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
                ConsumerRecords<String, String> records =
                        consumer.poll(Duration.ofMillis(1000));
                records.forEach(record -> {
                    Message newMessage = new Gson().fromJson(record.value(), Message.class);
                    if (newMessage.Timestamp == null) newMessage.Timestamp = new Date().toString();
                    this.ikc.addMessage(newMessage);
                    gui.addMessage(ComponentJList.kafkaInboundMessagesList, newMessage);
                    this.mlogger.WriteLog(new ServerLog(LogLevel.INFO, String.format("TKafkaConsumer Retrieved Message %s :%s: %s", newMessage.Source, newMessage.Action, newMessage.Target)));

                    TServerGui.kafkaInboundMessagesList.add(newMessage);
                    gui.numberOperation(GuiPanel.KAFKA, NumberLabel.nMessagesReceived, "+");
                });
            }
        }
        catch (Exception e)
        {
            e.printStackTrace();
            this.mlogger.WriteLog(new ServerLog(LogLevel.ERROR, String.format("TKafkaConsumer Connecting to Broker Error %s",e)));
        }
    }
}
