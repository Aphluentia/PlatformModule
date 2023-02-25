package kafka.Entities.Threads;

import com.google.gson.Gson;

import kafka.Entities.ILogger;
import kafka.Entities.Models.ServerLog;

import java.io.File;
import java.io.FileOutputStream;
import java.io.FileWriter;
import java.io.IOException;

public class TLogger extends Thread{

    private final ILogger ilogger;
    private final String filepath;

    /**
     * <b>Class Constructor</b>
     * <p>threadSuspended and stopFlag initialized as False</p>
     * @param _filepath: Interface  for the MKafka Monitor
     */
    public TLogger(ILogger _ilogger, String _filepath) {
        this.ilogger = _ilogger;
        this.filepath = _filepath;
    }


    @Override
    public void run() {
        try {
           while(true){
               ServerLog log = ilogger.waitForLog();
               WriteLog(log);
           }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
    public void WriteLog(ServerLog content){
        try {
            File file = new File(this.filepath);
            if (!file.exists()) {
                FileOutputStream fos = new FileOutputStream(file);
                fos.close();
            }
            FileWriter fw = new FileWriter(file, true);
            fw.write((new Gson()).toJson(content, ServerLog.class)+"\n");
            fw.close();

        } catch (IOException e) {
            e.printStackTrace();
        }
    }
}
