using Backend.Configs;
using Microsoft.Extensions.Options;
using System.Text;

namespace Backend.Helpers
{
    public class BridgeHelper
    {
       
        public static async Task<bool> CreateConnection(string WebPlatformId, Uri address)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = address;
            var content = new StringContent($"{{\"webPlatformId\": \"{WebPlatformId}\"}}", Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync("/api/ConnectionManager/Connection", content);

            if (response.IsSuccessStatusCode)
            {

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
