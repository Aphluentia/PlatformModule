using Bridge.Dtos.Enum;
using System.Net.Sockets;

namespace Bridge.Dtos.Entities
{
    public class SocketConnection
    {
        public TcpClient? ClientSocket { get; set; }
        public ModuleType ModuleType { get; set; }
        public string? WebPlatformId { get; set; }

        public string ToJson()
        {
            return $"{{ ClientSocket: {ClientSocket?.Client.RemoteEndPoint}, " +
                $"\nModuleType: {ModuleType.GetName(this.ModuleType)}, " +
                $"\nWebPlatformId: {this.WebPlatformId}}}";
        }
        public static ICollection<string> ToJson(ICollection<SocketConnection> connections)
        {
            return connections.Select(s => s.ToJson()).ToList();
        }
    }
    
}
