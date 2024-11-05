using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Core.Domain.AggregateRoots.Manager;
using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using EMS.Judge.Common.Services;
using EMS.Judge.Common.ViewModels;
using EMS.Judge.Services;
using Prism.Commands;
using Prism.Services.Dialogs;

namespace EMS.Judge.Views.Dialogs.Startlists;

public class StartlistDialogModel : DialogBase
{
    private readonly IExecutor<ManagerRoot> contestExecutor;
    private readonly IInputHandler input;
    private Dictionary<int, Startlist> startlists;
    private bool isOneEnabled;
    private bool isTwoEnabled;
    private bool isThreeEnabled;
    private bool isFourEnabled;
    private bool isFiveEnabled;
    private bool isSixEnabled;

    public StartlistDialogModel(
        IExecutor<ManagerRoot> contestExecutor,
        ISimplePrinter simplePrinter,
        IInputHandler input
    )
    {
        this.contestExecutor = contestExecutor;
        this.input = input;
        this.SelectAll = new DelegateCommand(() => this.RenderList(0));
        this.SelectOne = new DelegateCommand(() => this.RenderList(1));
        this.SelectTwo = new DelegateCommand(() => this.RenderList(2));
        this.SelectThree = new DelegateCommand(() => this.RenderList(3));
        this.SelectFour = new DelegateCommand(() => this.RenderList(4));
        this.SelectFive = new DelegateCommand(() => this.RenderList(5));
        this.SelectSix = new DelegateCommand(() => this.RenderList(6));
        this.Print = new DelegateCommand<Visual>(simplePrinter.Print);
    }

    public void HandleScroll(object sender, MouseWheelEventArgs mouseEvent)
    {
        this.input.HandleScroll(sender, mouseEvent);
    }

    public DelegateCommand SelectAll { get; }
    public DelegateCommand SelectOne { get; }
    public DelegateCommand SelectTwo { get; }
    public DelegateCommand SelectThree { get; }
    public DelegateCommand SelectFour { get; }
    public DelegateCommand SelectFive { get; }
    public DelegateCommand SelectSix { get; }
    public ObservableCollection<StartTemplateModel> List { get; } = new();
    public DelegateCommand<Visual> Print { get; }

    public override void OnDialogOpened(IDialogParameters parameters)
    {
        this.startlists = this.contestExecutor.Execute(x => x.GetStartList(), false);
        this.IsOneEnabled = this.startlists.ContainsKey(1);
        this.IsTwoEnabled = this.startlists.ContainsKey(2);
        this.IsThreeEnabled = this.startlists.ContainsKey(3);
        this.IsFourEnabled = this.startlists.ContainsKey(4);
        this.IsFiveEnabled = this.startlists.ContainsKey(5);
        this.IsSixEnabled = this.startlists.ContainsKey(6);
        this.RenderList(0);
    }

    private void RenderList(int stage)
    {
        var startlist =
            stage == 0
                ? new Startlist(this.startlists.SelectMany(x => x.Value))
                : this.startlists[stage];
        var templates = startlist.Select(x => new StartTemplateModel(x));
        this.List.Clear();
        this.List.AddRange(templates);
    }

    public bool IsOneEnabled
    {
        get => this.isOneEnabled;
        set => this.SetProperty(ref this.isOneEnabled, value);
    }
    public bool IsTwoEnabled
    {
        get => this.isTwoEnabled;
        set => this.SetProperty(ref this.isTwoEnabled, value);
    }
    public bool IsThreeEnabled
    {
        get => this.isThreeEnabled;
        set => this.SetProperty(ref this.isThreeEnabled, value);
    }
    public bool IsFourEnabled
    {
        get => this.isFourEnabled;
        set => this.SetProperty(ref this.isFourEnabled, value);
    }
    public bool IsFiveEnabled
    {
        get => this.isFiveEnabled;
        set => this.SetProperty(ref this.isFiveEnabled, value);
    }
    public bool IsSixEnabled
    {
        get => this.isSixEnabled;
        set => this.SetProperty(ref this.isSixEnabled, value);
    }
}
