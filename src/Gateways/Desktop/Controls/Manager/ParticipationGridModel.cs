using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public class ParticipationGridModel : BindableBase
{
    private bool isReadonly;
    private readonly IExecutor executor;
    public ParticipationGridModel(Participation participation, bool isReadonly)
    {
        this.IsReadonly = isReadonly;
        this.executor = StaticProvider.GetService<IExecutor>();
        this.Participant = participation.Participant;
        this.Number = this.Participant.Number;

        this.CreatePerformanceColumns(participation);
        var notifyCollectionChanged = (INotifyCollectionChanged) this.Participant.LapRecords;
        notifyCollectionChanged.CollectionChanged += (sender, args) =>
        {
            this.CreatePerformanceColumns(participation);
        };

        var aggregate = participation.Aggregate();
        if (aggregate.IsDisqualified)
        {
            this.Color = new SolidColorBrush(Colors.Red);
            this.DisqualifyCode = aggregate.DisqualifiedCode;
        }
        this.Print = new DelegateCommand(this.PrintAction);
    }

    public DelegateCommand Print { get; }

    public bool IsReadonly
    {
        get => this.isReadonly;
        protected set => this.SetProperty(ref this.isReadonly, value);
    }

    private void CreatePerformanceColumns(Participation participation)
    {
        var viewModels = Performance
            .GetAll(participation)
            .Select(perf => new PerformanceColumnModel(perf, this.IsReadonly))
            .ToList();
        App.Current.Dispatcher.Invoke(delegate
        {
            this.Performances.Clear();
            this.Performances.AddRange(viewModels);
        });
        this.RaisePropertyChanged(nameof(Performances));
    }

    public string Number { get; }
    public string DisqualifyCode { get; }
    public Participant Participant { get; }
    public SolidColorBrush Color { get; } = new(Colors.Black);
    public ObservableCollection<PerformanceColumnModel> Performances { get; private set; } = new();
    public void PrintAction()
    {
        this.executor.Execute(
            () => { new ParticipationPrinter(this).PrintDocument(); },
            false);
    }
}
