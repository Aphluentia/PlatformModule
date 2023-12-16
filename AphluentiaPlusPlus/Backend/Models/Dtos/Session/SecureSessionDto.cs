using Backend.Models.Dtos.Secure;
using SystemGateway.Dtos.Enum;
using SystemGatewayAPI.Dtos.Entities.Database;

namespace Backend.Models.Dtos.Session
{
    public class SessionData
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public DateTime Expires { get; set; }
        public bool IsExpired => DateTime.UtcNow > Expires;
        public ICollection<ModuleSnapshot> ModuleSnapshots { get; set; } = new List<ModuleSnapshot>();
    }
}
