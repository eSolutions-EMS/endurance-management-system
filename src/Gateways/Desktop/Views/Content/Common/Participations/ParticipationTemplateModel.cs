using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participations;

public class ParticipationTemplateModel : ParticipantTemplateModelBase
{
    private readonly Action<int> selectAction;
    public ParticipationTemplateModel(
        IEnumerable<Performance> performances,
        Action<int> selectAction,
        bool isExpanded = false)
        : base(performances, true)
    {
        this.selectAction = selectAction;
        this.Select = new DelegateCommand(this.SelectAction);
        this.Print = new DelegateCommand(this.PrintAction);

        this.visibility = isExpanded
            ? Visibility.Visible
            : Visibility.Collapsed;
    }

    public DelegateCommand Print { get; }
    public DelegateCommand Select { get; }

    private Visibility visibility;

    private void PrintAction()
    {
        this.ToggleEditPerformanceVisibility();
        var printer = new PerfomancePrinter(this);
        printer.PreviewDocument();
        this.ToggleEditPerformanceVisibility();
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
