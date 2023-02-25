package kafka.Entities.Models;

import kafka.Entities.Enum.LogLevel;

import java.time.LocalDateTime;

public class ServerLog {
    public LogLevel level;
    public String datetime = LocalDateTime.now().toString();
    public String message;

    public ServerLog(LogLevel level, String message) {
        this.level = level;
        this.message = message;
    }

}
