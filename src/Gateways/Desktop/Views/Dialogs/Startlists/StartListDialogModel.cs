using EnduranceJudge.Domain.Aggregates.Manager;
using EnduranceJudge.Gateways.Desktop.Core.ViewModels;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Views.Dialogs.Startlists;

public class StartlistDialogModel : DialogBase
{
    private readonly IExecutor<ContestManager> contestExecutor;

    public StartlistDialogModel(IExecutor<ContestManager> contestExecutor, IPrinter printer)
    {
        this.contestExecutor = contestExecutor;
        this.GetList = new DelegateCommand(this.RenderList);
        this.Print = new DelegateCommand<Visual>(printer.Print);
    }

    public ObservableCollection<StartTemplateModel> List { get; } = new();
    public DelegateCommand GetList { get; }
    public DelegateCommand<Visual> Print { get; }
    public bool IncludePast { get; set; } = false;

    public override void OnDialogOpened(IDialogParameters parameters)
    {
        this.RenderList();
    }

    private void RenderList()
    {
        var participants = this.contestExecutor
            .Execute(x => x.GetStartList(this.IncludePast))
            .Select(x => new StartTemplateModel(x));
        this.List.Clear();
        this.List.AddRange(participants);
    }
}
