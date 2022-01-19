using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Common.PhasePerformances;
using EnduranceJudge.Localization.Translations;
using Prism.Commands;
using System.Windows;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participants;

public class ParticipantTemplateModel : ParticipantTemplateModelBase<PerformanceTemplateModel>, ICollapsable
{
    private readonly IPrinter printer;

    public ParticipantTemplateModel(Participant participant, bool isExpanded = true) : base(participant)
    {
        this.printer = StaticProvider.GetService<IPrinter>();
        this.ToggleVisibility = new DelegateCommand(this.ToggleVisibilityAction);
        this.Print = new DelegateCommand<Visual>(this.PrintAction);
        if (isExpanded)
        {
            this.ToggleVisibilityAction();
        }
    }

    public DelegateCommand<Visual> Print { get; }
    public DelegateCommand ToggleVisibility { get; }
    private string toggleText = Words.EXPAND;
    private readonly int number;
    private Visibility visibility = Visibility.Collapsed;

    private void PrintAction(Visual control)
    {
        this.printer.Print(control);
    }

    public Visibility Visibility
    {
        get => this.visibility;
        set => this.SetProperty(ref this.visibility, value);
    }
    public string ToggleText
    {
        get => this.toggleText;
        set => this.SetProperty(ref this.toggleText, value);
    }

    private void ToggleVisibilityAction()
    {
        if (this.Visibility == Visibility.Collapsed)
        {
            this.Visibility = Visibility.Visible;
            this.ToggleText = Words.COLLAPSE;
        }
        else
        {
            this.Visibility = Visibility.Collapsed;
            this.ToggleText = Words.EXPAND;
        }
    }
}
