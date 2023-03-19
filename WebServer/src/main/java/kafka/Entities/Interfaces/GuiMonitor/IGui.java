package kafka.Entities.Interfaces.GuiMonitor;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.ComponentJList;
import kafka.Entities.Enum.GuiPanel;
import kafka.Entities.Enum.NumberLabel;
import kafka.Entities.Models.ConnectionRequest;
import kafka.Entities.Models.Message;

public interface IGui {

    void numberOperation(GuiPanel panel, NumberLabel number, String action);
    void addMessage(ComponentJList panel, Message message);
    void removeMessage(ComponentJList panel, Message message);
    void addConnectionRequest(ComponentJList jlist, ApplicationType appType, ConnectionRequest message);
    void removeConnectionRequest(ComponentJList jlist, ApplicationType appType, ConnectionRequest message);
}
