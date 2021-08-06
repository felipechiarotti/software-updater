
namespace Model
{
    public static class Version
    {
        public static string ServerVersion;
        public static string LocalVersion;
        public static bool isUpdated(string localFile, string serverURL)
        {
            LocalVersion = FileHandler.GetFileContentAsString(localFile);
            ServerVersion = RequestHandler.GetServerVersion(serverURL);
            return LocalVersion == ServerVersion;
        }

    }
}
