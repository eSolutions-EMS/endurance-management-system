using EnduranceJudge.Application.Services;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Core.Static;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Imports
{
    public class ImportViewModel : ViewModelBase
    {
        private readonly IExplorerService explorer;
        private readonly INavigationService navigation;
        private readonly IImportService importService;

        public ImportViewModel(IExplorerService explorer, INavigationService navigation, IImportService importService)
        {
            this.explorer = explorer;
            this.navigation = navigation;
            this.importService = importService;
            this.OpenFolderDialog = new DelegateCommand(this.OpenFolderDialogAction);
            this.OpenImportFileDialog = new DelegateCommand(this.OpenImportFileDialogAction);
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

        private void OpenFolderDialogAction()
        {
            var selectedPath = this.explorer.SelectDirectory();
            if (selectedPath == null)
            {
                return;
            }
            this.WorkDirectoryPath = selectedPath;
            // var selectWorkFileRequest = new SelectWorkFile
            // {
            //     DirectoryPath = selectedPath,
            // };
            //
            // this.WorkDirectoryVisibility = Visibility.Hidden;
            // this.ImportFilePathVisibility = Visibility.Visible;
            //
            // var isNewFileCreated = await this.application.Execute(selectWorkFileRequest);
            // if (!isNewFileCreated)
            // {
            //     this.Redirect();
            // }
        }

        private void OpenImportFileDialogAction()
        {
            var path = this.explorer.SelectFile();
            if (path == null)
            {
                return;
            }

            this.ImportFilePath = path;
            this.importService.Import(path);
            // this.Redirect();
        }

        private void Redirect()
        {
            this.navigation.NavigateToEvent();
        }
    }
}
