using Bridge.Dtos.Entities;
using System.Net.Sockets;

namespace Bridge.BackgroundService.Interfaces
{
    public interface IConnectionManagerMessageHandler
    {
        public Connection? CreateConnection(Connection _connection);
        public Connection? CloseConnection(Connection _connection);
        public Connection? CheckConnection(Connection _connection);
        public Connection? AddMessage(Connection _connection, Message _message);
    }
}
