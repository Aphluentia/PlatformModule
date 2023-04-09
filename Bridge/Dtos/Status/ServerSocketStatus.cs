using Bridge.Dtos.Entities;

namespace Bridge.Dtos.Status
{
    public class ServerSocketStatus
    {
        public ICollection<SocketServer> Servers {get;set;} = new List<SocketServer>();
        public int WithstandingConnectionsCounter { get; set; } = 0;
        public ICollection<string> WithstandingConnections { get; set; } = new List<string>();
    }
}
