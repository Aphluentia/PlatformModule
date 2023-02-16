namespace WebPlatform.Services
{
    public interface ICLogger
    {
        public void Info(string message, string? clientId);
        public void Warn(string message, string? clientId);
        public void Error(string message, string? clientId);
        public void System(string message, string? clientId);
        public void Info(string message);
        public void Warn(string message);
        public void Error(string message);
        public void System(string message);

    }
}
