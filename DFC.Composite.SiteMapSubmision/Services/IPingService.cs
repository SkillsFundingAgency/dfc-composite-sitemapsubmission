using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Composite.SiteMapSubmision.Services
{
    public interface IPingService
    {
        Task<HttpResponseMessage> Ping(string url);
    }
}
