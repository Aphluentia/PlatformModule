
using Backend.Models.Dtos.Base;
using Backend.Models.Entities;

namespace Backend.Models.Dtos
{
    public class GetDashboardDataOutputDto
    {
       
        public SessionData SessionData { get; set; }
        public string QrCodeData{ get; set; }
        public ICollection<BridgeDiscovery> Server { get; set; }
    }
}
