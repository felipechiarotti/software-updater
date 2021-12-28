
using System.Net.Http;
using System.Threading.Tasks;

namespace Model
{
    public static class Version
    {
        public static string ServerVersion;
        public static string LocalVersion;

        public static async Task<bool> IsUpdated(string serverURL, string localFile)
        {
            var serverTask = RequestHandler.GetServerVersion(serverURL);
            LocalVersion = FileHandler.GetFileContentAsString(localFile);

            var task = await serverTask;
            ServerVersion = task.Content.ReadAsStringAsync().Result;
            return LocalVersion == ServerVersion;
        }

    }
}
