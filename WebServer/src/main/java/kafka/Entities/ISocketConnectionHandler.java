package kafka.Entities;

public interface ISocketConnectionHandler {
    Message retrieveMessage(AppType appType);
}
