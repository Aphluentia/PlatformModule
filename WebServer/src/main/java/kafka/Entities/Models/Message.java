package kafka.Entities.Models;

public class Message {
    public String WebPlatformId;
    public String ApplicationType;
    public String Action;
    public double Timestamp;
    public String Message;

    public Message(String webPlatformId, String applicationType, String action, double timestamp, String message) {
        WebPlatformId = webPlatformId;
        ApplicationType = applicationType;
        Action = action;
        Timestamp = timestamp;
        Message = message;
    }

    public Message(String webPlatformId, String applicationType, String action, double timestamp) {
        WebPlatformId = webPlatformId;
        ApplicationType = applicationType;
        Action = action;
        Timestamp = timestamp;
    }

    @Override
    public String toString() {
        return "Message{" +
                "WebPlatformId='" + WebPlatformId + '\'' +
                ", ApplicationType='" + ApplicationType + '\'' +
                ", Action='" + Action + '\'' +
                ", Timestamp=" + Timestamp +
                ", Message='" + Message + '\'' +
                '}';
    }
}
