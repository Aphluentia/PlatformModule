package kafka.Entities.Models;
import kafka.Entities.Enum.*;


public class Message {
    public String Source, Target;
    public ConnectionAction Action;
    public ApplicationType SourceApplicationType, TargetApplicationType;
    public double Timestamp;
    public String Message;

    public Message(String source, String target, ConnectionAction action, ApplicationType sourceApplicationType, ApplicationType targetApplicationType, double timestamp, String message) {
        Source = source;
        Target = target;
        Action = action;
        SourceApplicationType = sourceApplicationType;
        TargetApplicationType= targetApplicationType;
        Timestamp = timestamp;
        Message = message;
    }

    public Message(String source, String target,ApplicationType sourceApplicationType, ApplicationType targetApplicationType, ConnectionAction action, double timestamp) {
        Source = source;
        SourceApplicationType = sourceApplicationType;
        TargetApplicationType= targetApplicationType;
        Target = target;
        Action = action;
        Timestamp = timestamp;
    }

    @Override
    public String toString() {
        return "Message{" +
                "Source='" + Source + '\'' +
                ", Target='" + Target + '\'' +
                ", Action=" + Action +
                ", SourceApplicationType=" + SourceApplicationType +
                ", TargetApplicationType=" + TargetApplicationType +
                ", Timestamp=" + Timestamp +
                ", Message='" + Message + '\'' +
                '}';
    }
}
