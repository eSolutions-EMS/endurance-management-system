using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participants;

public class ParticipationTemplateModel : ParticipantTemplateModelBase
{
    private readonly IPrinter printer;
    private readonly Action<int> selectAction;
    public ParticipationTemplateModel(
        int number,
        IEnumerable<Performance> performances,
        Action<int> selectAction,
        bool isExpanded = false)
        : base(number, performances, true)
    {
        this.selectAction = selectAction;
        this.printer = StaticProvider.GetService<IPrinter>();
        this.Select = new DelegateCommand(this.SelectAction);
        this.Print = new DelegateCommand<Visual>(this.PrintAction);

        this.visibility = isExpanded
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public DelegateCommand<Visual> Print { get; }
    public DelegateCommand Select { get; }

    private Visibility visibility;

    private void PrintAction(Visual control)
    {
        this.printer.Print(control);
    }

    public Visibility Visibility
    {
        get => this.visibility;
        set => this.SetProperty(ref this.visibility, value);
    }

    private void SelectAction()
    {
        this.selectAction(this.Number);
    }
}
