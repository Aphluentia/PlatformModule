using Bridge.BackgroundService.Interfaces;
using Bridge.BackgroundService.Monitors;
using Bridge.Dtos.Entities;
using Bridge.Dtos.Enum;
using Bridge.Dtos.Status;
using Bridge.Providers;
using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace Bridge.BackgroundService.Threads
{
    public class TServerSocket
    {
        private BackgroundWorker _worker;
        private bool StopFlag;
        private readonly IConnectionManagerSocketServer _connectionManager;

        private readonly IPAddress ipAddress;
        private ServerSocketStatus _status;
        public readonly int _port;
        public readonly ModuleType _moduleType;
        public TServerSocket(ServerSocketStatus status, MConnectionManager mConnectionManager, ModuleType moduleType, int port)
        {
            _status = status;
            _connectionManager = mConnectionManager;
            _moduleType = moduleType;
            _port = port;
            ipAddress = IPAddress.Any;
            StopFlag = false;
            _worker = new BackgroundWorker();
            _worker.DoWork += RunAsync;
            _worker.RunWorkerCompleted += RunWorkerCompleted;
        }

        public void Start()
        {
            _worker.RunWorkerAsync();
        }

        private void RunAsync(object sender, DoWorkEventArgs e)
        {
            TcpListener listener = new TcpListener(ipAddress, _port);
            listener.Start();
            while (!StopFlag)
            {
                TcpClient client = listener.AcceptTcpClient();
                
                if (_connectionManager.AddWithstandingConnection(client))
                {
                    _status.WithstandingConnections.Add(client.Client.RemoteEndPoint.ToString());
                    _status.WithstandingConnectionsCounter++;
                }
            }
        }

        private void RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
    }
}
