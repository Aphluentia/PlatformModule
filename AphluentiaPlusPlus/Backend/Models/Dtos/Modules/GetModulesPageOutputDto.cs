using Backend.Models.Entities;

namespace Backend.Models.Dtos.Modules
{
    public class GetModulesPageOutputDto
    {
        public ICollection<ModuleTypeDetails> Modules { get; set; }
        public string QrCodeUrl { get; set; }
    }
}
