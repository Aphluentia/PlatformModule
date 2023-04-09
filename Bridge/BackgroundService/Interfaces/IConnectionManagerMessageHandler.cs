using Bridge.Dtos.Entities;
using System.Net.Sockets;

namespace Bridge.BackgroundService.Interfaces
{
    public interface IConnectionManagerMessageHandler
    {
        public SocketConnection? CreateConnection(Connection _connection);
        public SocketConnection? CloseConnection(Connection _connection);
        public SocketConnection? PingConnection(Connection _connection);
        public SocketConnection? UpdateSection(Connection _connection);
    }
}
