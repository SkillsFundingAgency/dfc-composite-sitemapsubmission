using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Composite.SiteMapSubmision.Services
{
    public class PingService : IPingService
    {
        private readonly HttpClient _httpClient;

        public PingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> Ping(string url)
        {
            var msg = new HttpRequestMessage(HttpMethod.Get, url);
            var response = await _httpClient.SendAsync(msg);
            response.EnsureSuccessStatusCode();
            return response;
        }
    }
}
