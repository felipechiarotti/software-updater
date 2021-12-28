using Model;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using Updater;
using WpfAnimatedGif;

namespace View
{
    public partial class MainWindow : Window
    {
        private UpdaterController controller;
        private eStatus currentImageStatus;
        public MainWindow()
        {
            InitializeComponent();
            InitializeAndRun("python-3.9.0-amd64-webinstall");
        }

        private void InitializeAndRun(string programName)
        {
            InitializeUpdaterController();
            _ = StartUpdater(programName);
        }

        private void InitializeUpdaterController()
        {
            controller = new UpdaterController("https://paraisodovapor.com/updates/software/version.txt", "https://paraisodovapor.com/updates/software", "bin", "version.dat", UpdateUI);

        }

        private async Task StartUpdater(string programName)
        {
            await controller.runUpdater(programName);
            Application.Current.Shutdown();
        }

        private void UpdateUI(string title, string description, eStatus status, string downloadSize = "")
        {
            this.lbTitle.Text = title;
            this.lbDescription.Text = description;
            this.lbDownloadSize.Text = downloadSize;
            if(currentImageStatus != status)
                updateImageStatus(status);
            this.currentImageStatus = status;
        }

        private void updateImageStatus(eStatus status)
        {
            switch (status)
            {
                case eStatus.SearchingUpdates:
                    updateImage("spin.gif");
                    break;
                case eStatus.DownloadingUpdates:
                    updateImage("download-install.gif");
                    break;
                case eStatus.FinishedUpdates:
                    updateImage("starting.gif");
                    break;
                case eStatus.Error:
                    updateImage("error.png");
                    break;
            }
        }

        private void updateImage(string fileName)
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri($"pack://application:,,,/Images/{fileName}");
            image.EndInit();
            ImageBehavior.SetAnimatedSource(this.image, image);
        }

        private void CloseButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                Application.Current.Shutdown();
        }

        private void TitleWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
