﻿using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participants;

public class ParticipantTemplateModel : ParticipantTemplateModelBase
{
    private readonly IPrinter printer;
    private readonly Action<int> selectAction;
    public ParticipantTemplateModel(Participant participant, Action<int> selectAction, bool isExpanded = false)
        : base(participant, true)
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
