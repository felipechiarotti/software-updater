
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Model
{
    public static class RequestHandler
    {

        public static async Task<HttpResponseMessage> GetServerVersion(string uri)
        {
            using (var client = new HttpClient())
            {
                return await client.GetAsync(uri);
            }
        }

        public static async Task DownloadLatestVersion(string uri, string pathToSave, DownloadProgressChangedEventHandler progressChangedEvent)
        {
            using (var webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += progressChangedEvent;
                await webClient.DownloadFileTaskAsync(new Uri(uri), pathToSave);
            }

            
        }

    }
}
