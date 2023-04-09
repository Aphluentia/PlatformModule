using Bridge.BackgroundService.Interfaces;
using Bridge.Dtos.Entities;
using Bridge.Dtos.Status;
using Bridge.Providers;
using Confluent.Kafka;
using System.Data.Common;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace Bridge.BackgroundService.Monitors
{
    public class MConnectionManager : IConnectionManagerMessageHandler, IConnectionManagerSocketServer
    {
        private ConnectionManagerProvider ConnectionManagerProvider { get; set; }
        private ConnectionProviderStatus _status { get; set; }
        private Dictionary<string, Socket> WithstandingConnections { get; set; }

        // Reentrant Lock
        private readonly object _lock = new object();
        public MConnectionManager(ConnectionManagerProvider provider, ConnectionProviderStatus status)
        {
            this.ConnectionManagerProvider = provider;
            this._status = status;
            WithstandingConnections = new Dictionary<string, Socket>();
        }
        public SocketConnection? CreateConnection(Connection _connection)
        {
            SocketConnection? socketConnection = null;
            lock (_lock)
            {
                try
                {
                    if ((socketConnection = ConnectionManagerProvider.GetSocketConnection(_connection)) != null)
                    {
                        
                        ConnectionManagerProvider.AddConnection(_connection);
                        _status.ConnectionsCounter++;
                    }
                }
                catch (Exception)
                {
                    return null;
                }
               
            }
            return socketConnection;
        }
        public SocketConnection? CloseConnection(Connection _connection)
        {
            SocketConnection? socketConnection = null;
            lock (_lock)
            {
                try
                {
                    Connection conn;
                    if (!string.IsNullOrEmpty(_connection.ModuleId) && (conn = ConnectionManagerProvider.GetModuleConnection(_connection.ModuleId)) != null)
                    {
                        ConnectionManagerProvider.RemoveConnection(_connection.ModuleId);
                        _status.ConnectionsCounter--;
                        socketConnection = ConnectionManagerProvider.GetSocketConnection(conn);
                    }
                }
                catch (Exception)
                {
                    return null;
                }

            }
            return socketConnection;
        }

        public SocketConnection? PingConnection(Connection _connection)
        {
            SocketConnection? socketConnection = null;
            lock (_lock)
            {
                try
                {
                    Connection? conn = null;
                    if (!string.IsNullOrEmpty(_connection.ModuleId) && (conn = ConnectionManagerProvider.GetModuleConnection(_connection.ModuleId)) != null)
                    {
                        socketConnection = ConnectionManagerProvider.GetSocketConnection(conn);
                    }

                }
                catch (Exception)
                {
                    return null;
                }

            }
            return socketConnection;
        }

        public SocketConnection UpdateSection(Connection _connection)
        {
            SocketConnection? socketConnection = null;
            lock (_lock)
            {
                try
                {
                    Connection? conn = null;
                    if (!string.IsNullOrEmpty(_connection.ModuleId) && (conn = ConnectionManagerProvider.GetModuleConnection(_connection.ModuleId)) != null)
                    {
                        socketConnection = ConnectionManagerProvider.GetSocketConnection(conn);
                    }

                }
                catch (Exception)
                {
                    return null;
                }

            }
            return socketConnection;
        }
     

        // Socket Server Operations
        public bool AddWithstandingConnection(TcpClient _client)
        {
            bool flag = false;
            lock (_lock)
            {
                try
                {
                    if (!ConnectionManagerProvider.ExistsWithstandingSocketConnection(_client))
                    {

                        ConnectionManagerProvider.AddUnprocessedSocketConnection(_client);
                        _status.ConnectionsCounter++;
                        flag = true;
                    }
                    
                }
                catch (Exception)
                {
                    return false;
                }

            }
            return flag;
        }

        
    }
}
