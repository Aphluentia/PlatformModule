package kafka.Entities.Models;

import kafka.Entities.Enum.Action;
import kafka.Entities.Enum.AppType;

public class ConnectionRequest {
    private Action action;
    private AppType appType;

    @Override
    public String toString() {
        return "ConnectionRequest{" +
                "action=" + action +
                ", appType=" + appType +
                '}';
    }

    public Action getAction() {
        return action;
    }

    public void setAction(Action action) {
        this.action = action;
    }

    public AppType getAppType() {
        return appType;
    }

    public void setAppType(AppType appType) {
        this.appType = appType;
    }
}
