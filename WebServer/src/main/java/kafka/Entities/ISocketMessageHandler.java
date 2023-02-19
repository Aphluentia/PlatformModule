package kafka.Entities;

public interface ISocketMessageHandler {
    void addMessage(String message, AppType appType);
}
