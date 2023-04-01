
using Backend.Models.Entities;

namespace Backend.Models.Dtos
{
    public class GetModulesSessionInformationOutputDto
    {
        public SessionData SessionData { get; set; }
        public string QrCodeB64 { get; set; }
    }
}
