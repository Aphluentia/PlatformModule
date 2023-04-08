using Bridge.Dtos.Enum;
using System.Net.Sockets;

namespace Bridge.Dtos.Entities
{
    public class SocketConnection
    {
        public Socket ClientSocket { get; set; }
        public ModuleType ModuleType { get; set; }
        public string WebPlatformId { get; set; }
    }
}
