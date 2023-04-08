using Bridge.Dtos.Entities;
using Bridge.Dtos.Enum;
using System.Net.Sockets;
using System.Reflection;

namespace Bridge.Providers
{
    public class ConnectionManagerProvider
    {

        private ICollection<Connection> connections;

        private ICollection<SocketConnection> socketConnections;
        public ConnectionManagerProvider()
        {
            connections = new HashSet<Connection>();
            socketConnections = new HashSet<SocketConnection>();
        }
        public Connection? GetWebPlatformConnection(string WebPlatformId)
        {
            return connections.FirstOrDefault(x => x.WebPlatformId == WebPlatformId);
        }
        public Connection? GetModuleConnection(string ModuleId)
        {
            return connections.FirstOrDefault(x => x.ModuleId == ModuleId);
        }
        public Connection? GetModuleTypeConnection(ModuleType ModuleType)
        {
            return connections.FirstOrDefault(x => x.ModuleType == ModuleType);
        }
        public bool ExistsWebPlatformModuleConnection(string WebPlatformId, ModuleType ModuleType)
        {
            return connections.FirstOrDefault(x => x.ModuleType == ModuleType && x.WebPlatformId == WebPlatformId) != null ? true : false;
        }
        public bool AddConnection(string WebPlatformId, string ModuleId, ModuleType ModuleType)
        {
            if (this.GetModuleConnection(ModuleId)!=null || this.ExistsWebPlatformModuleConnection(WebPlatformId, ModuleType))
            {
                return false;
            }
            connections.Add(new Connection()
            {
                ModuleType = ModuleType,
                WebPlatformId = WebPlatformId,
                ModuleId = ModuleId
            });
            return true;
        }
        public Connection RemoveConnection(string sourceId)
        {
            Connection conn = null;
            if ((conn = GetModuleConnection(sourceId)) != null)
            {
                connections.Remove(conn);
            }
            
            return conn;
        }

        // Socket Connections
        public SocketConnection? GetSocketConnection(string webPlatformId, ModuleType moduleType)
        {
            return this.socketConnections.FirstOrDefault(x => x.WebPlatformId == webPlatformId && x.ModuleType == moduleType);
        }
        public bool AddSocketConnection(string WebPlatformId, Socket clientSocketAddress, ModuleType moduleType)
        {
            if (GetSocketConnection(WebPlatformId, moduleType) != null)
            {
                socketConnections.Remove(socketConnections.First(x => x.WebPlatformId == WebPlatformId && x.ModuleType == moduleType));
            }
            socketConnections.Add(new SocketConnection()
            {
                ModuleType = moduleType,
                WebPlatformId = WebPlatformId,
                ClientSocket = clientSocketAddress
            });
            return true;
        }

      
    }
}
