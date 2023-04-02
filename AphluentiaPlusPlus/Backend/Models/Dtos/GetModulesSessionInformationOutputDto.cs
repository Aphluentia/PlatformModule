
using Backend.Models.Entities;

namespace Backend.Models.Dtos
{
    public class GetModulesSessionInformationOutputDto
    {
        public SessionData SessionData { get; set; }
        public string qrCodeData{ get; set; }
        public int messageServerPort { get; set; }
    }
}
