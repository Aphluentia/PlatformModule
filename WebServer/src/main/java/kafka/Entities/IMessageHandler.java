package kafka.Entities;

import kafka.Entities.Models.Message;

public interface IMessageHandler {
    Message handleMessage();
}
