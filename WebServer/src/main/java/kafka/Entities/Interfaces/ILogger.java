package kafka.Entities.Interfaces;

import kafka.Entities.Models.ServerLog;

public interface ILogger {
    ServerLog waitForLog();
}
