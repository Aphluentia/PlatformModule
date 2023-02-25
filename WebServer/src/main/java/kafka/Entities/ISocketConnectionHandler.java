package kafka.Entities;

import kafka.Entities.Enum.AppType;
import kafka.Entities.Models.Message;

public interface ISocketConnectionHandler {
    Message retrieveMessage(AppType appType);
}
