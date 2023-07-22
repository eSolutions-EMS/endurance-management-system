using Accessibility;
using EMS.Judge.Print.Performances;
using EMS.Judge.Services;
using Core.Utilities;
using Core.Domain.AggregateRoots.Common.Performances;
using Core.Domain.AggregateRoots.Manager.Aggregates;
using Core.Domain.State.Participants;
using Core.Domain.State.Participations;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Media;

namespace EMS.Judge.Controls.Manager;

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
        this.Distance = $"({participation.CompetitionConstraint.Laps.Sum(x => x.LengthInKm)})";
        var completedLaps = participation.Participant.LapRecords.Count(x => x.Result != null);
        this.IsComplete = completedLaps == participation.CompetitionConstraint.Laps.Count
            || participation.Participant.LapRecords.Any(x => x.Result?.IsNotQualified ?? false);

        this.CreatePerformanceColumns(participation);
        var notifyCollectionChanged = (INotifyCollectionChanged) this.Participant.LapRecords;
        notifyCollectionChanged.CollectionChanged += (sender, args) =>
        {
            this.CreatePerformanceColumns(participation);
        };

        Participation.UpdateEvent += (_, x) =>
        {
            App.Current.Dispatcher.Invoke(delegate
            {
                if (x.Id == participation.Id)
                {
                    this.CheckColor(x);
                }
            });
        };
        CheckColor(participation);
        this.Print = new DelegateCommand(this.PrintAction);
    }

    private void CheckColor(Participation participation)
    {
        var aggregate = participation.Aggregate();
        if (aggregate.IsDisqualified)
        {
            this.Color = new SolidColorBrush(Colors.Red);
            this.DisqualifyCode = aggregate.DisqualifiedCode;
        }
        else if (this.IsComplete)
        {
            this.Color = new SolidColorBrush(Colors.Green);
        }
        else
        {
            this.Color = new SolidColorBrush(Colors.Black);
            this.DisqualifyCode = null;
        }
    }

    public DelegateCommand Print { get; }

    public bool IsReadonly
    {
        get => this.isReadonly;
        protected set => this.SetProperty(ref this.isReadonly, value);
    }

    private void CreatePerformanceColumns(Participation participation)
    {
        var performances = Performance.GetAll(participation).ToList();
        var columnModels = new List<PerformanceColumnModel>();
        for (var i = 0; i < performances.Count; i++)
        {
            var model = new PerformanceColumnModel(performances[i], i + 1, this.IsReadonly);
            columnModels.Add(model);
        }
        App.Current.Dispatcher.Invoke(delegate
        {
            this.Performances.Clear();
            this.Performances.AddRange(columnModels);
        });
        this.RaisePropertyChanged(nameof(Performances));
    }

    public bool IsComplete { get; }
    public string Number { get; }
    public string Distance { get; protected set; }
    private string disqualifyCode;
    public string DisqualifyCode
    {
        get => this.disqualifyCode;
        private set => this.SetProperty(ref this.disqualifyCode, value);
    }
    public Participant Participant { get; }
    private SolidColorBrush color = new(Colors.Black);
    public SolidColorBrush Color
    {
        get => this.color;
        private set => this.SetProperty(ref this.color, value);
    }
    public ObservableCollection<PerformanceColumnModel> Performances { get; private set; } = new();
    public void PrintAction()
    {
        this.executor.Execute(
            () => { new ParticipationPrinter(this).PrintDocument(); },
            false);
    }
}
