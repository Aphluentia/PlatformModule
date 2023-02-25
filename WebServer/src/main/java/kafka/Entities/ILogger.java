package kafka.Entities;

import kafka.Entities.Models.ServerLog;

public interface ILogger {
    ServerLog waitForLog();
}
