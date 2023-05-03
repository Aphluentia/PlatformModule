using Bridge.Dtos.Entities;
using Newtonsoft.Json;

namespace Bridge.Dtos.Status
{
    public class ConnectionProviderStatus
    {
        public int ConnectionsCounter = 0;
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}
