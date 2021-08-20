using EnduranceJudge.Application.Import.ImportFromFile;
using EnduranceJudge.Application.Import.WorkFile;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Commands;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System.Threading.Tasks;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Import
{
    public class ImportViewModel : ViewModelBase
    {
        private readonly IExplorerService explorer;
        private readonly INavigationService navigation;
        private readonly IApplicationService application;

        public ImportViewModel(
            IExplorerService explorer,
            INavigationService navigation,
            IApplicationService application)
        {
            this.explorer = explorer;
            this.navigation = navigation;
            this.application = application;
            this.OpenFolderDialog = new AsyncCommand(this.OpenFolderDialogAction);
            this.OpenImportFileDialog = new AsyncCommand(this.OpenImportFileDialogAction);
        }

        private string workDirectoryPath;
        public string WorkDirectoryPath
        {
            get => this.workDirectoryPath;
            private set => this.SetProperty(ref this.workDirectoryPath, value);
        }

        private Visibility importVisibility = Visibility.Hidden;
        public Visibility ImportVisibility
        {
            get => this.importVisibility;
            set => this.SetProperty(ref this.importVisibility, value);
        }
        private string importFilePath;
        public string ImportFilePath
        {
            get => this.importFilePath;
            set => this.SetProperty(ref this.importFilePath, value);
        }

        public DelegateCommand OpenFolderDialog { get; }
        public DelegateCommand OpenImportFileDialog { get; }

        private async Task OpenFolderDialogAction()
        {
            var selectedPath = this.explorer.SelectDirectory();
            if (selectedPath == null)
            {
                return;
            }

            this.WorkDirectoryPath = selectedPath;

            var selectWorkFileRequest = new SelectWorkFile
            {
                DirectoryPath = selectedPath,
            };

            var isNewFileCreated = await this.application.Execute(selectWorkFileRequest);
            if (isNewFileCreated)
            {
                this.ImportVisibility = Visibility.Visible;
            }
            else
            {
                this.Redirect();
            }
        }

        private async Task OpenImportFileDialogAction()
        {
            var path = this.explorer.SelectFile();
            if (path == null)
            {
                return;
            }

            this.ImportFilePath = path;

            var importFromFileRequest = new ImportFromFile
            {
                FilePath = path,
            };

            await this.application.Execute(importFromFileRequest);

            this.Redirect();
        }

        private void Redirect()
        {
            this.navigation.NavigateToEvent();
        }
    }
}
