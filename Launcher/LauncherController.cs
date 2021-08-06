using System;
using Updater;

namespace Launcher
{
    public class LauncherController
    {
        private UpdaterController Controller;
        public LauncherController(string serverURL, string serverFilesURL, string fileVersionName, Action<string> infoMessage)
        {
            Controller = new UpdaterController(serverURL, serverFilesURL, fileVersionName, infoMessage);
        }

        public void runUpdater()
        {
            Controller.runUpdater();
        }

        public void runProgram(string fileExeName)
        {
            Controller.runProgram(fileExeName);
        }
    }
}
