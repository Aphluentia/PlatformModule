using Bridge.Dtos.Entities;

namespace Bridge.BackgroundService.Interfaces
{
    public interface IConnectionManagerMessageHandler
    {
        public SocketConnection CreateConnection(Message _message);
        public SocketConnection CloseConnection(Message _message);
        public SocketConnection PingConnection(Message _message);
        public SocketConnection UpdateSection(Message _message);
    }
}
