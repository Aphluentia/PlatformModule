using Bridge.Dtos.Entities;
using System.Net.Sockets;

namespace Bridge.BackgroundService.Interfaces
{
    public interface IConnectionManagerSocketServer
    {
        public bool AddWithstandingConnection(TcpClient _client);
    }
}
