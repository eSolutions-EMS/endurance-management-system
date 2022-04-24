﻿using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Core.Components.XAML;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Controls.Manager;
using Prism.Commands;
using System.Windows;
using System.Windows.Controls;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Participations;

public class ParticipationTemplateModel : ParticipantTemplateModelBase
{
    private readonly IExecutor executor;
    public ParticipationTemplateModel(Participation participation, IExecutor executor)
        : base(participation)
    {
        this.executor = executor;
        this.Print = new DelegateCommand(this.PrintAction);
        this.Shrink = new DelegateCommand<UIElement>(this.ShrinkAction);
    }

    private void ShrinkAction(UIElement root)
    {
        this.RecursiveShrink(root);
    }

    private void RecursiveShrink(UIElement element)
    {
        this.HandleScalable(element);
        if (element is Decorator decorator)
        {
            this.RecursiveShrink(decorator.Child);
        }
        else if (element is Panel panel)
        {
            foreach (var child in panel.Children)
            {
                if (child is UIElement uiElement)
                {
                    this.RecursiveShrink(uiElement);
                }
            }
        }
        else if (element is ItemsControl itemsControl)
        {
            foreach (var item in itemsControl.Items)
            {
                if (item is UIElement uiElement)
                {
                    this.RecursiveShrink(uiElement);
                }
            }
        }
    }

    private void HandleScalable(UIElement element)
    {
        if (element is IScalableElement scalableElement)
        {
            scalableElement.ScaleDown(50);
        }
    }

    public DelegateCommand<UIElement> Shrink { get; }
    public DelegateCommand Print { get; }

    private void PrintAction()
    {
        this.executor.Execute(() =>
        {
            this.ControlsVisibility = Visibility.Collapsed;
            var printer = new PerfomancePrinter(this);
            printer.PreviewDocument();
        });
        this.ControlsVisibility = Visibility.Visible;
    }
}
