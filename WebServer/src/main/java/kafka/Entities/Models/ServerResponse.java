package kafka.Entities.Models;

import kafka.Entities.Enum.ApplicationType;
import kafka.Entities.Enum.ConnectionAction;

public class ServerResponse {
    public String moduleId;
    public String platformId;
    public ApplicationType SourceApplicationType;
    public ConnectionAction action;
    public String data;
    public ServerResponse(ConnectionAction action, String moduleId, String platformId, ApplicationType sourceApplicationType, String data) {
        this.action = action;
        this.moduleId = moduleId;
        this.platformId = platformId;
        SourceApplicationType = sourceApplicationType;
        this.data = data;
    }
}
