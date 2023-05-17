
using Backend.Models.Dtos.Base;
using Backend.Models.Entities;
using Backend.Models.Enums;

namespace Backend.Models.Dtos
{
    public class ModulesOutputDto
    {
       
        public string QrCodeData{ get; set; }
        public ICollection<ModuleTypeDto> Modules { get; set; }
    }
}
