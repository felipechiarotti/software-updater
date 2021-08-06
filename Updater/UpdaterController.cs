using System;
using Model;

using System.Diagnostics;


namespace Updater
{
    public class UpdaterController
    {
        private string ServerVersionURL;
        private string ServerFilesURL;
        private string BasePath;
        private string FileVersionName;
        private string PathToExtract;
        private InfoMessageDelegate InfoMessage;


        public UpdaterController(string serverURL, string serverFilesURL, string fileVersionName, string pathToExtract, Action<string> infoMessage)
        {
            ServerVersionURL = serverURL;
            ServerFilesURL = serverFilesURL;
            BasePath = FileHandler.GetCurrentDirectory()+"\\";
            FileVersionName = fileVersionName;
            PathToExtract = pathToExtract+"\\";
            InfoMessage = new InfoMessageDelegate(infoMessage != null ? infoMessage : (string text)=>{ });
        }

        public void runUpdater()
        {
            InfoMessage("Procurando por atualizações...");
            if (!Model.Version.isUpdated(FileVersionName, ServerVersionURL))
            {
                updateSoftware();
            }
        }

        private void updateSoftware()
        {
            InfoMessage("Baixando a versão: " + Model.Version.ServerVersion);
            downloadUpdate();

            InfoMessage($"Atualizando da versão {Model.Version.LocalVersion} para {Model.Version.ServerVersion}");
            updateLocalFiles();
        }

        private  void downloadUpdate()
        {
            RequestHandler.DownloadLatestVersion(ServerFilesURL + "/" + Model.Version.ServerVersion + ".zip", BasePath + "\\" + Model.Version.ServerVersion + ".zip");
            FileHandler.ExtractZip(BasePath + "\\" + Model.Version.ServerVersion + ".zip", BasePath + PathToExtract + Model.Version.ServerVersion);
        }

        private void updateLocalFiles()
        {
            var filesName = FileHandler.GetFilesInFolder(BasePath + PathToExtract +Model.Version.ServerVersion);

            FileHandler.DeleteFile(BasePath+PathToExtract + filesName);

            FileHandler.MoveFilesFromTo(BasePath+ PathToExtract + Model.Version.ServerVersion, BasePath+ PathToExtract, filesName);
            FileHandler.DeleteDirectory(BasePath + PathToExtract + Model.Version.ServerVersion);
            FileHandler.DeleteFile(Model.Version.ServerVersion + ".zip");
            FileHandler.OverwriteFile(FileVersionName, Model.Version.ServerVersion);


        }
        public void runProgram(string exeFileName)
        {
            InfoMessage("Iniciando o programa");
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = BasePath + PathToExtract,
                FileName = BasePath + PathToExtract +exeFileName
            };
            Process.Start(startInfo);
            System.Environment.Exit(0);
        }

        public delegate void InfoMessageDelegate(string text);

    }
}
