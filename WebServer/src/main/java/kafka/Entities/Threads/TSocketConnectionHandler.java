package kafka.Entities.Threads;

import com.google.gson.Gson;
import kafka.Entities.AppType;
import kafka.Entities.ISocketConnectionHandler;
import kafka.Entities.Message;
import org.apache.kafka.clients.consumer.ConsumerRecords;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.net.Socket;
import java.time.Duration;

public class TSocketConnectionHandler extends Thread {

    private final String webPlatformId;
    private final AppType appType;
    private final ISocketConnectionHandler mWebSockets;
    /**
     * Server Port
     */
    private int port;
    /**
     * Boolean flag for stopping process
     */
    private boolean stopFlag;
    /**
     * Server socket
     */
    private Socket clientSocket;
    private DataInputStream dis;
    private DataOutputStream dos;

    /**
     * Initialize TServer
     *
     * @param _mserver MServer Monitor Request Receiver Interface
     * @param _port    Server Port
     */
    public TSocketConnectionHandler(ISocketConnectionHandler _mserver, String _webPlatformId,AppType _appType, Socket _socket, DataInputStream _dis, DataOutputStream _dos) {
        this.stopFlag = false;
        this.mWebSockets = _mserver;
        this.webPlatformId = _webPlatformId;
        this.clientSocket = _socket;
        this.dos = _dos;
        this.dis = _dis;
        this.appType = _appType;


    }

    /**
     * <p>stopFlag flag set to true, process ends</p>
     */
    public void stopProcess() {
        this.stopFlag = true;
        try {
            clientSocket.close();
        } catch (IOException e) {
            System.out.println("Error Closing ServerSocket At Server With Port " + String.valueOf(this.port));
        }
    }

    /**
     * Start running socket, send and receive information
     */
    @Override
    public void run() {
        try {
            while(!this.stopFlag){
                Message message = this.mWebSockets.retrieveMessage(this.appType);
                System.out.println(message.toString());
            }

        } catch (Exception e) {
            System.out.println(e);
        }
    }

}


