
using System.Net;
using System.Net.Http;


namespace Model
{
    public static class RequestHandler
    {

        public static  string GetServerVersion(string uri)
        {
            var response = "";
            using (var client = new HttpClient())
            {
                response = client.GetAsync(uri).Result.Content.ReadAsStringAsync().Result;
            }
            return response;
        }

        public static void DownloadLatestVersion(string uri, string pathToSave)
        {
            using (var webClient = new WebClient())
            {
                webClient.DownloadFile(uri, pathToSave);
            }

            
        }

    }
}
