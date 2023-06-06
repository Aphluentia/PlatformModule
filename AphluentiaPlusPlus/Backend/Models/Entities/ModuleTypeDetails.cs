using Backend.Models.Enums;
using ZXing.QrCode.Internal;

namespace Backend.Models.Entities
{
    public class ModuleTypeDetails
    {

        public string Name => Enum.GetName(Code);
        public ModuleType Code { get; set; }                
    }
}
