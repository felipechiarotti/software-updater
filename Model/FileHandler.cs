using System.Linq;

namespace Model
{
    public static class FileHandler
    {
        public static string GetFileContentAsString(string path)
        {
            return System.IO.File.ReadAllText(path);
        }

        public static void OverwriteFile(string path, string text)
        {
            System.IO.File.WriteAllText(path,text);
        }

        public static void ExtractZip(string zipPath, string extractPath)
        {
             System.IO.Compression.ZipFile.ExtractToDirectory(zipPath, extractPath);
        }

        public static void DeleteFile(string path)
        {
            System.IO.File.Delete(path);
        }

        public static void DeleteFile(string[] paths)
        {
            foreach (var path in paths)
            {
                System.IO.File.Delete(path);
            }
        }

        public static void DeleteDirectory(string path)
        {
            System.IO.Directory.Delete(path);
        }

        public static void MoveFilesFromTo(string from, string to, string[] fileName)
        {
            foreach (var file in fileName)
            {
                System.IO.File.Move(from+"\\"+file,to + "\\" + file);
            }
        }

        public static string[] GetFilesInFolder(string path)
        {
            return System.IO.Directory.GetFiles(path).Select(x => x.Substring(x.LastIndexOf('\\')+1, x.Length-x.LastIndexOf('\\')-1)).ToArray();
        }

        public static string GetCurrentDirectory()
        {
            return System.IO.Directory.GetCurrentDirectory();
        }
    }
}
