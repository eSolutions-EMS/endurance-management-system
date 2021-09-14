using EnduranceJudge.Application.Import.ImportFromFile;
using EnduranceJudge.Application.Import.WorkFile;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using EnduranceJudge.Gateways.Desktop.Core.Services;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Import
{
    public class ImportViewModel : ViewModelBase
    {
        private readonly IServiceProvider provider;
        private readonly IExplorerService explorer;
        private readonly INavigationService navigation;
        private readonly IApplicationService application;

        public ImportViewModel(
            IServiceProvider provider,
            IExplorerService explorer,
            INavigationService navigation,
            IApplicationService application)
        {
            this.provider = provider;
            this.explorer = explorer;
            this.navigation = navigation;
            this.application = application;
            this.OpenFolderDialog = new AsyncCommand(this.OpenFolderDialogAction);
            this.OpenImportFileDialog = new AsyncCommand(this.OpenImportFileDialogAction);
        }

        public DelegateCommand OpenFolderDialog { get; }
        public DelegateCommand OpenImportFileDialog { get; }

        private string workDirectoryPath;
        private string importFilePath;
        private Visibility workDirectoryVisibility = Visibility.Visible;
        private Visibility importFilePathVisibility = Visibility.Hidden;

        public string WorkDirectoryPath
        {
            get => this.workDirectoryPath;
            private set => this.SetProperty(ref this.workDirectoryPath, value);
        }
        public Visibility WorkDirectoryVisibility
        {
            get => this.workDirectoryVisibility;
            set => this.SetProperty(ref this.workDirectoryVisibility, value);
        }
        public string ImportFilePath
        {
            get => this.importFilePath;
            set => this.SetProperty(ref this.importFilePath, value);
        }
        public Visibility ImportFilePathVisibility
        {
            get => this.importFilePathVisibility;
            set => this.SetProperty(ref this.importFilePathVisibility, value);
        }

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

            this.WorkDirectoryVisibility = Visibility.Hidden;
            this.ImportFilePathVisibility = Visibility.Visible;

            var isNewFileCreated = await this.application.Execute(selectWorkFileRequest);
            if (!isNewFileCreated)
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
