package kafka.Entities.Enum;

import java.util.HashMap;

public class ServerConfig {
    public static String BOOTSTRAP_SERVERS;
    public static int NO_KAFKA_CONSUMERS;
    public static int NO_MESSAGE_HANDLERS;
    public static String TOPIC;
    public static String LOGS_FILEPATH;
    public static HashMap<AppType, Integer> MODULES_PORT = new HashMap<>();
    public static int SOCKET_HUB_PORT = 9005;

    public static void DefaultConfig(){
        ServerConfig.BOOTSTRAP_SERVERS = "localhost:8095, localhost:8096,localhost:8097";
        ServerConfig.NO_KAFKA_CONSUMERS = 1;
        ServerConfig.NO_MESSAGE_HANDLERS = 1;
        ServerConfig.TOPIC = "kafkabroker";
        ServerConfig.LOGS_FILEPATH = "Logs.txt";
        int base = 9006;
        for (AppType app: AppType.values()) {
            ServerConfig.MODULES_PORT.put(app, base);
            base++;
        }
        ServerConfig.SOCKET_HUB_PORT = 9005;
    }

}

