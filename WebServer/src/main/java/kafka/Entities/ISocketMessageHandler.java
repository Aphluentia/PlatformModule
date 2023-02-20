package kafka.Entities;

public interface ISocketMessageHandler {
    void addMessage(Message message, AppType appType);
}
