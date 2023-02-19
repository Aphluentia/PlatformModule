import kafka.Entities.IWebSocketHub;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.ServerSocket;
import java.net.Socket;

public class TWebSocketHub extends Thread {
    /**
     * MServer Monitor Request Receiver Interface
     */
    private final IWebSocketHub mserver;
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
    private ServerSocket serverSocket;

    /**
     * Initialize TServer
     * @param _mserver MServer Monitor Request Receiver Interface
     * @param _port Server Port
     */
    public TWebSocketHub(IWebSocketHub _mserver, int _port) {
        this.stopFlag = false;
        this.mserver = _mserver;
        this.port = _port;
        try {
            serverSocket = new ServerSocket(port);
        }
        catch (IOException e) {
            e.printStackTrace();
        }
    }

    /**
     * <p>stopFlag flag set to true, process ends</p>
     */
    public void stopProcess() {
        this.stopFlag = true;
        try {
            serverSocket.close();
        }
        catch (IOException e) {
            System.out.println("Error Closing ServerSocket At Server With Port "+String.valueOf(this.port));
        }
    }

    /**
     * Start running socket, send and receive information
     */
    @Override
    public void run() {
        try {
           while(true){
                System.out.println("Waiting for the client request");
                //creating socket and waiting for client connection
                Socket socket = serverSocket.accept();
                //read from socket to ObjectInputStream object
                ObjectInputStream ois = new ObjectInputStream(socket.getInputStream());
                //convert ObjectInputStream object to String
                String message = (String) ois.readObject();
                System.out.println("Message Received: " + message);
                //create ObjectOutputStream object
                ObjectOutputStream oos = new ObjectOutputStream(socket.getOutputStream());
                //write object to Socket
                oos.writeObject("Hi Client "+message);
                //close resources
                ois.close();
                oos.close();
                socket.close();
                //terminate the server if client sends exit request
                if(message.equalsIgnoreCase("exit")) break;
            }
        catch (Exception e)
        {
            System.out.println("Error Creating RequestHandler At Server With Port "+String.valueOf(this.port));
        }
    }
}