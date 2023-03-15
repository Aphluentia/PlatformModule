package kafka.Entities.Interfaces.GuiMonitor;

import kafka.Entities.Enum.GuiPanel;
import kafka.Entities.Enum.NumberLabel;

public interface IGui {

    void numberOperation(GuiPanel panel, NumberLabel number, String action);
}
