using System.Net;
using System.Net.Http;

namespace NetCore.Hosting
{
    public static class HostingFactory
    {
        public static HttpClient HttpClient { get; }

        static HostingFactory()
        {
            var cookieContainer = new CookieContainer();
            var httpClientHandler = new HttpClientHandler() { CookieContainer = cookieContainer };
            HttpClient = new HttpClient(httpClientHandler);
        }
    }
}
