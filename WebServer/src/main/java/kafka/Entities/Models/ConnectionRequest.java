package kafka.Entities.Models;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.ConnectionAction;

public class ConnectionRequest {
    public ConnectionAction Action;
    public String PlatformId;
    public ApplicationType applicationType = null;
    public String ModuleId = null;

    @Override
    public String toString() {
        return "ConnectionRequest{" +
                "Action=" + Action +
                ", Platform=" + PlatformId +
                '}';
    }

}
