using Bridge.Dtos.Entities;
using Bridge.Dtos.Enum;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Reflection;

namespace Bridge.Providers
{

    // Stores Messages to Be Polled
    // Adds Messages according to WebPlatformID
    public class ConnectionManagerProvider
    {

        public ICollection<Connection> connections;
        public ConnectionManagerProvider()
        {
            connections = new HashSet<Connection>();
        }
        public bool RegisterWebPlatformConnection(string WebPlatformId)
        {
            var modules = ((IList<ModuleType>)Enum.GetValues(typeof(ModuleType))).Where(c=>c!=ModuleType.AphluentiaPlusPlus_Web);
            foreach (ModuleType mType in modules)
            {
                var connection = new Connection { WebPlatformId = WebPlatformId, ModuleType = mType, Messages = new List<Message>() };
                if (connections.Any(c => c.WebPlatformId == WebPlatformId && c.ModuleType == mType)) return false;
                connections.Add(connection);
            }
            return true;
        }
        public bool RemoveWebPlatformConnection(string WebPlatformId)
        {
            if (!connections.Any(c => c.WebPlatformId == WebPlatformId))
                return false;
            var filteredConnections = connections.Where(c=> c.WebPlatformId == WebPlatformId).ToList();
            filteredConnections.ForEach(c => connections.Remove(c));
            return true;
        }
        public Connection? AddMessage(Connection connection, Message message)
        {

            var conn = connections.ToList().Find(c => c.ModuleId == connection.ModuleId && c.WebPlatformId == connection.WebPlatformId);
            if (conn != null)
                conn.Messages.Add(message);
            return conn;
        }

        // If Last Message is CloseConnection remove
        public ICollection<Message>? PollMessages(string WebPlatformId, ModuleType ModuleType)
        {
            if (!connections.Any(c => c.WebPlatformId == WebPlatformId && c.ModuleType == ModuleType))
                return null;
            var filteredConnections = connections.ToList().Find(c => c.WebPlatformId == WebPlatformId && c.ModuleType == ModuleType);
            var messages = filteredConnections.Messages;
            if (filteredConnections.Messages.Any(m => m.Action == Dtos.Enum.Action.Close_Connection))
            {

                (connections).Remove(filteredConnections);
                var cleanConnection = Connection.Reset(filteredConnections);
                connections.Add(cleanConnection);
            }
            
            return messages;
        }



        public Connection? CreateConnection(Connection newBrokerConnection)
        {

            if (!connections.Any(c => (c.WebPlatformId == newBrokerConnection.WebPlatformId && c.ModuleType == newBrokerConnection.ModuleType) || c.ModuleId == newBrokerConnection.ModuleId))
                return null;
            var con = connections.ToList().Find(c => c.WebPlatformId == newBrokerConnection.WebPlatformId && c.ModuleType == newBrokerConnection.ModuleType);
            con.ModuleId = newBrokerConnection.ModuleId;
            return con;
            
        }
        public Connection? CloseConnection(Connection newBrokerConnection)
        {

            if (!connections.Any(c => (c.WebPlatformId == newBrokerConnection.WebPlatformId && c.ModuleType == newBrokerConnection.ModuleType) || c.ModuleId == newBrokerConnection.ModuleId))
                return null;

            var con = connections.ToList().Find(c => c.WebPlatformId == newBrokerConnection.WebPlatformId && c.ModuleType == newBrokerConnection.ModuleType);
            con.ModuleId = null;
            return con;

        }
        public Connection? CheckConnection(Connection newBrokerConnection)
        {
            return connections.FirstOrDefault(c => c.WebPlatformId == newBrokerConnection.WebPlatformId && c.ModuleType == newBrokerConnection.ModuleType && c.ModuleId == newBrokerConnection.ModuleId);
            
        }
        

    }
}
