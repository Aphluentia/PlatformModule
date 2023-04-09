using Bridge.BackgroundService.Monitors;
using Bridge.BackgroundService.Threads;
using Bridge.ConfigurationSections;
using Bridge.Dtos.Entities;
using Bridge.Dtos.Enum;
using Bridge.Dtos.Status;
using Microsoft.Extensions.Options;

namespace Bridge.Providers
{
    public class SocketServerProvider
    {
        private readonly int _port;
        private readonly MConnectionManager _mConnectionManager;
        private ServerSocketStatus _status;
        public Dictionary<ModuleType, TServerSocket> _serverSockets;
        public ICollection<SocketServer> _servers;
        public SocketServerProvider(MConnectionManager mConnectionManager, IOptions<ServerSocketConfigSection> config, ServerSocketStatus status)
         {
            this._port = config.Value.InitialPort;
            this._mConnectionManager = mConnectionManager;
            this._status = status;
            _serverSockets = new Dictionary<ModuleType, TServerSocket>();
            _servers = new HashSet<SocketServer>();
        }
        public bool StartServers()
        {
            var modules = new List<ModuleType> { ModuleType.Mobile_App, ModuleType.Bedroom_App };
        
            for (int i = 0; i < modules.Count(); i++)
            {
                try
                {
                    var socketServer = new TServerSocket(_status, _mConnectionManager, modules[i],_port + i);
                    socketServer.Start();
                    _serverSockets[modules[i]] = socketServer;
                    _servers.Add(new SocketServer { ModuleType = modules[i], Port = _port + i });
                }
                catch (Exception)
                {
                    return false;
                } 
            }
            return true;
        }
    }
}
