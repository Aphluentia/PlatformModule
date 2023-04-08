using Bridge.BackgroundService.Interfaces;
using Bridge.Dtos.Entities;
using Bridge.Dtos.Status;
using Bridge.Providers;
using Confluent.Kafka;
using System.Runtime.CompilerServices;

namespace Bridge.BackgroundService.Monitors
{
    public class MConnectionManager : IConnectionManagerMessageHandler
    {
        private ConnectionManagerProvider ConnectionManagerProvider { get; set; }

        // Reentrant Lock
        private readonly object _lock = new object();
        public MConnectionManager(ConnectionManagerProvider provider)
        {
            this.ConnectionManagerProvider = provider;
        }
        public SocketConnection CreateConnection(Message _message)
        {
            SocketConnection socketConnection = null;
            lock (_lock)
            {
                // Use Monitor.Enter() to acquire the lock
                Monitor.Enter(_lock);

                try
                {
                    if ((socketConnection = ConnectionManagerProvider.GetSocketConnection(_message.TargetId, _message.SourceModuleType)) != null)
                    {
                        ConnectionManagerProvider.AddConnection(_message.TargetId, _message.SourceId, _message.SourceModuleType);
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
                finally
                {
                    // Use Monitor.Exit() to release the lock
                    Monitor.Exit(_lock);
                }
            }
            return socketConnection;
        }
        public bool CloseConnection(Message _message)
        {
            lock (_lock)
            {
                // Use Monitor.Enter() to acquire the lock
                Monitor.Enter(_lock);

                try
                {
                    if ((ConnectionManagerProvider.ExistsWebPlatformModuleConnection(_message.TargetId, _message.SourceModuleType)) != null)
                    {
                        ConnectionManagerProvider.RemoveConnection(_message.TargetId, _message.SourceId);
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    // Use Monitor.Exit() to release the lock
                    Monitor.Exit(_lock);
                }
            }
            return socketConnection;
        }

        public bool PingConnection()
        {
            throw new NotImplementedException();
        }

        public bool UpdateSection()
        {
            throw new NotImplementedException();
        }
    }
}
