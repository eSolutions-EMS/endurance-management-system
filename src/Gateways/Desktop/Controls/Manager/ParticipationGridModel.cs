using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Participations;
using EnduranceJudge.Gateways.Desktop.Print.Performances;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;

namespace EnduranceJudge.Gateways.Desktop.Controls.Manager;

public class ParticipationGridModel : BindableBase
{
    private readonly int? columns;
    private readonly IExecutor executor;
    public ParticipationGridModel(Participation participation, bool isReadonly, int? columns = null)
    {
        this.IsReadonly = isReadonly;
        this.columns = columns;
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

    public bool IsReadonly { get; protected set; }

    private void CreatePerformanceColumns(Participation participation)
    {
        var viewModels = Performance
            .GetAll(participation)
            .Select(perf => new PerformanceColumnModel(perf, this.IsReadonly))
            .ToList();
        if (this.columns.HasValue)
        {
            this.EmptyColumns = columns.Value - viewModels.Count;
            // TODO fix this.
            // It does not work, because we only render columns using the
            // ItemsControl with ItemsSource set to Performances
        }
        if (this.EmptyColumns < 0)
        {
            throw new Exception($"Participant {this.Number} has more performances than columns.");
        }
        App.Current.Dispatcher.Invoke((Action) delegate
        {
            this.Performances.Clear();
            this.Performances.AddRange(viewModels);
        });
        this.RaisePropertyChanged(nameof(Performances));
    }

    public int EmptyColumns { get; private set; }
    public string Number { get; }
    public string DisqualifyCode { get; }
    public Participant Participant { get; }
    public SolidColorBrush Color { get; } = new(Colors.Black);
    public ObservableCollection<PerformanceColumnModel> Performances { get; private set; } = new();
    public void PrintAction()
    {
        this.executor.Execute(() =>
        {
            this.IsReadonly = true;
            var printer = new ParticipationPrinter(this);
            printer.PreviewDocument();
            this.IsReadonly = false;
        }, false);
    }
}
