using EnduranceJudge.Application.Imports.Commands;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Objects;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Imports
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
            var importFromFileRequest = new Import
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
