using System;
using Model;

using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

using System.ComponentModel;

namespace Updater
{
    public class UpdaterController
    {
        private string ServerVersionURL;
        private string ServerFilesURL;
        private string BasePath;
        private string PathToExtract;
        private string LocalVersionFile;
        private InfoMessageDelegate InfoMessage;

        private bool Retry;


        public UpdaterController(string serverURL, string serverFilesURL, string pathToExtract, string localVersionFilePath, Action<string, string, eStatus, string> infoMessage)
        {
            ServerVersionURL = serverURL;
            ServerFilesURL = serverFilesURL;
            BasePath = FileHandler.GetCurrentDirectory() + "\\";
            PathToExtract = pathToExtract + "\\";
            LocalVersionFile = localVersionFilePath;
            InfoMessage = new InfoMessageDelegate(infoMessage);
        }

        public async Task runUpdater(string exeToExecute)
        {
            InfoMessage("Procurando atualizações", "Aguarde um momento", eStatus.SearchingUpdates);
            try
            {
                if (!await Model.Version.IsUpdated(ServerVersionURL, LocalVersionFile))
                {
                    await updateSoftware();
                }
                runProgram(exeToExecute);
            }
            catch (WebException)
            {
                InfoMessage("Erro", "Problemas de conexão", eStatus.Error);
            }
            catch (Win32Exception)
            {
                InfoMessage("Erro", "Não foi possível iniciar o programa", eStatus.Error);
            }
            catch (Exception ex)
            {
                InfoMessage("Erro", "Falha ao instalar atualizações", eStatus.Error, ex.Message);
                ClearUpdateFiles();
                UpdateVersionFile(Model.Version.LocalVersion);
                Retry = true;
            }
            finally
            {

                   
                await Task.Delay(2500);
                if(Retry)
                    InfoMessage("Erro", "Tentando novamente em 3 segundos", eStatus.SearchingUpdates);
                await Task.Delay(2500);
                if (Retry )
                {
                    await runUpdater(exeToExecute);
                    Retry = false;
                }
            }
        }

        private async Task updateSoftware()
        {

            await DownloadUpdate();

            InfoMessage("Instalando Atualizações", $"Atualizando da versão {Model.Version.LocalVersion} para {Model.Version.ServerVersion}", eStatus.InstallingUpdates);
            updateLocalFiles();
        }

        private async Task DownloadUpdate()
        {
            await RequestHandler.DownloadLatestVersion(ServerFilesURL + "/" + Model.Version.ServerVersion + ".zip", BasePath + "\\" + Model.Version.ServerVersion + ".zip", (object sender, DownloadProgressChangedEventArgs e) =>
            {
                InfoMessage("Baixando Atualizações", $"{e.ProgressPercentage}% Baixado", eStatus.DownloadingUpdates, $"{e.BytesReceived / 1024} / {e.TotalBytesToReceive / 1024}KB");

            });


            InfoMessage("Instalando Atualizações", "Extraindo arquivos", eStatus.DownloadingUpdates);
            FileHandler.ExtractZip(BasePath + "\\" + Model.Version.ServerVersion + ".zip", BasePath + PathToExtract + Model.Version.ServerVersion);
        }

        private void updateLocalFiles()
        {
            var filesName = FileHandler.GetFilesInFolder(BasePath + PathToExtract + Model.Version.ServerVersion);

            InfoMessage("Instalando Atualizações", "Aplicando arquivos atualizados", eStatus.DownloadingUpdates);

            FileHandler.MoveFilesFromTo(BasePath + PathToExtract + Model.Version.ServerVersion, BasePath + PathToExtract, filesName);
            ClearUpdateFiles();
            UpdateVersionFile(Model.Version.ServerVersion);
        }

        private void ClearUpdateFiles()
        {
            var filesName = FileHandler.GetFilesInFolder(BasePath + PathToExtract + Model.Version.ServerVersion);
            FileHandler.DeleteFile(BasePath + PathToExtract + filesName);
            FileHandler.DeleteDirectory(BasePath + PathToExtract + Model.Version.ServerVersion);
            FileHandler.DeleteFile(Model.Version.ServerVersion + ".zip");
        }

        private void UpdateVersionFile(string version)
        {
            FileHandler.OverwriteFile(Model.Version.ServerVersion, LocalVersionFile);
        }


        private void runProgram(string exeFileName)
        {
            InfoMessage("Atualização Completa", "Iniciando a aplicação", eStatus.FinishedUpdates);
            var startInfo = new ProcessStartInfo
            {
                WorkingDirectory = BasePath + PathToExtract,
                FileName = BasePath + PathToExtract + exeFileName
            };
            Process.Start(startInfo);
        }



        public delegate void InfoMessageDelegate(string title, string description, eStatus status, string downloadSize = "");


    }
}
