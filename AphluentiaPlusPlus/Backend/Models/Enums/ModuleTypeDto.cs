

namespace Backend.Models.Enums
{
    public class ModuleTypeDto
    {
        public string Name { get; set; }
        public ModuleType Code { get; set; }
        public static ModuleTypeDto FromModuleType(ModuleType moduleType)
        {
            return new ModuleTypeDto { Name = Enum.GetName(moduleType), Code = moduleType};
        }
    }
}
