using Microsoft.WindowsAPICodePack.Dialogs;

namespace EnduranceJudge.Gateways.Desktop.Core.Services.Implementations
{
    public class ExplorerService : IExplorerService
    {
        public string SelectDirectory()
        {
            using var openFolderDialog = new CommonOpenFileDialog
            {
                IsFolderPicker = true
            };

            return this.GetPath(openFolderDialog);
        }

        public string SelectFile()
        {
            using var selectFileDialog = new CommonOpenFileDialog();

            return this.GetPath(selectFileDialog);
        }

        private string GetPath(CommonOpenFileDialog dialog)
        {
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                return dialog.FileName;
            }

            return null;
        }
    }
}
