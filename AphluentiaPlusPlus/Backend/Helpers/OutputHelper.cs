using Backend.Models.Dtos.Base;
using Newtonsoft.Json;

namespace Backend.Helpers
{
    public class OutputHelper
    {
        public static OutputDto GetOutputMessage(object? output)

        {
            var success = (output != null);
            return new OutputDto()
            {
                Metadata = new Metadata()
                {
                    Success = success
                },
                Data = output
            };
        }
    }
}
