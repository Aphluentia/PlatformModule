using System.Net.Http;
using static QRCoder.PayloadGenerator;

namespace Backend.Helpers
{
    public static class HttpHelper
    {
        public static async Task<(bool, string?)> Get(string url)
        {
            var response = await new HttpClient().GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, responseContent);
        }

        public static async Task<(bool, string?)> Post(string url, object body)
        {
            var response = await new HttpClient().PostAsJsonAsync(url, body);
            var responseContent = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, responseContent);
        }

        public static async Task<(bool, string?)> Put(string url, object body)
        {
            var response = await new HttpClient().PutAsJsonAsync(url, body);
            var responseContent = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, responseContent);
        }

        public static async Task<(bool, string?)> Delete(string url)
        {
            var response = await new HttpClient().DeleteAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();
            return (response.IsSuccessStatusCode, responseContent);
        }
    }
}
