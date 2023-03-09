package kafka.Entities.Models;

import kafka.Entities.Enum.ApplicationType;

public class ServerResponse {
    public String moduleId;
    public String platformId;
    public ApplicationType SourceApplicationType;
    public String data;
    public ServerResponse(String moduleId, String platformId, ApplicationType sourceApplicationType, String data) {
        this.moduleId = moduleId;
        this.platformId = platformId;
        SourceApplicationType = sourceApplicationType;
        this.data = data;
    }
}
