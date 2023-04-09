using Bridge.Dtos.Entities;
using Bridge.Dtos.Enum;
using System.Net;
using System.Net.Sockets;
using System.Reflection;

namespace Bridge.Providers
{
    public class ConnectionManagerProvider
    {

        public ICollection<Connection> connections;

        public ICollection<SocketConnection> socketConnections;

        public ICollection<TcpClient> withstandingSocketConnections;
        public ConnectionManagerProvider()
        {
            connections = new HashSet<Connection>();
            socketConnections = new HashSet<SocketConnection>();
            withstandingSocketConnections = new HashSet<TcpClient>();
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
        public bool AddConnection(Connection connection)
        {
            if ((connection.ModuleId!= null && this.GetModuleConnection(connection.ModuleId)!=null) || 
                (connection.WebPlatformId!=null && this.ExistsWebPlatformModuleConnection(connection.WebPlatformId, connection.ModuleType)))
            {
                return false;
            }
            connections.Add(connection);
            return true;
        }
        public Connection? RemoveConnection(string sourceId)
        {
            Connection? conn = null;
            if ((conn = GetModuleConnection(sourceId)) != null)
            {
                connections.Remove(conn);
            }
            
            return conn;
        }

        // Socket Connections
        public SocketConnection? GetSocketConnection(Connection connection)
        {
            return this.socketConnections.FirstOrDefault(x => x.WebPlatformId == connection.WebPlatformId && x.ModuleType == connection.ModuleType);
        }
        // Pair a socket channel with a webplatformid and a module type
        public bool ProcessSocketConnection(string WebPlatformId, string clientSocketAddress, ModuleType moduleType)
        {
            TcpClient? socket = null;
            if ((socket = withstandingSocketConnections.FirstOrDefault(x => ((IPEndPoint?)x?.Client.RemoteEndPoint)?.ToString() == clientSocketAddress)) == null) return false;

            if (this.socketConnections.FirstOrDefault(x => x.WebPlatformId == WebPlatformId && x.ModuleType == moduleType) != null)
            {
                socketConnections.Remove(socketConnections.First(x => x.WebPlatformId == WebPlatformId && x.ModuleType == moduleType));
            }
            socketConnections.Add(new SocketConnection()
            {
                ModuleType = moduleType,
                WebPlatformId = WebPlatformId,
                ClientSocket = socket
            });
            return true;
        }

        // Add new socket connection, each module server socket will have a diff address for the clients
        public bool AddUnprocessedSocketConnection(TcpClient clientSocket)
        {
            if (this.withstandingSocketConnections.FirstOrDefault(x => ((IPEndPoint?)x?.Client.RemoteEndPoint)?.ToString() == ((IPEndPoint?)clientSocket.Client.RemoteEndPoint)?.ToString()) != null)
            {
                return false;
            }
            withstandingSocketConnections.Add(clientSocket);
            return true;
        }
        public bool ExistsWithstandingSocketConnection(TcpClient clientSocket)
        {
            if (this.withstandingSocketConnections.FirstOrDefault(x => ((IPEndPoint?)x?.Client.RemoteEndPoint)?.ToString() == ((IPEndPoint?)clientSocket.Client.RemoteEndPoint)?.ToString()) != null)
            {
                return true;
            }
            return false;
        }


    }
}
