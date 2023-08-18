using SystemGateway.Dtos.SecurityManager;

namespace Backend.Models.Entities
{
    public class ExpirationData
    {

        public DateTime Expires { get; set; }
        public bool IsExpired { get; set; }
        public int TimeLeft { get; set; }
        public static ExpirationData FromSecurityData(SecurityDataDto dto)
        {
            return new ExpirationData
            {
                Expires = dto.Expires,
                IsExpired = dto.IsExpired,
                TimeLeft = dto.TimeLeft
            };
        }
    }
}
