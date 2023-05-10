using EnduranceJudge.Application.Core;
using EnduranceJudge.Domain.AggregateRoots.Configuration;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State.Laps;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Core;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Laps;

// TODO: Change IsFinal to checkbox
public class LapViewModel : NestedConfigurationBase<LapView, Lap>, ILapState
{
    private readonly IExecutor<ConfigurationRoot> executor;
    private string isFinalText;
    private bool isFinal;
    private double? lengthInKm;
    private int? orderBy;
    private int? maxRecoveryTimeInMinutes;
    private int? restTimeInMinutes;
    private bool requireCompulsoryInspection;

    private LapViewModel() : this(null, null) { }
    public LapViewModel(IExecutor<ConfigurationRoot> executor, IQueries<Lap> laps) : base(laps)
    {
        this.executor = executor;
    }

    protected override IDomain Persist()
    {
        if (this.ParentId.HasValue)
        {
            return this.executor.Execute(
                config => config.Laps.Create(this.ParentId.Value, this),
                true);
        }
        else
        {
            return this.executor.Execute(
                config => config.Laps.Update(this),
                true);
        }
    }

    public bool IsFinal
    {
        get => this.isFinal;
        set => this.SetProperty(ref this.isFinal, value);
    }
    public string IsFinalText
    {
        get => this.isFinalText;
        set => this.SetProperty (ref this.isFinalText, value);
    }
    public double? LengthInKmDisplay
    {
        get => this.lengthInKm;
        set => this.SetProperty(ref this.lengthInKm, value);
    }
    public int? OrderByDisplay
    {
        get => this.orderBy;
        set => this.SetProperty(ref this.orderBy, value);
    }
    public int? MaxRecoveryTimeInMinsDisplay
    {
        get => this.maxRecoveryTimeInMinutes;
        set => this.SetProperty(ref this.maxRecoveryTimeInMinutes, value);
    }
    public int? RestTimeInMinsDisplay
    {
        get => this.restTimeInMinutes;
        set => this.SetProperty(ref this.restTimeInMinutes, value);
    }

    public double LengthInKm
    {
        get => this.LengthInKmDisplay ?? default;
        set => this.LengthInKmDisplay = value;
    }
    public int OrderBy
    {
        get => this.OrderByDisplay ?? default;
        set => this.OrderByDisplay = value;
    }
    public int MaxRecoveryTimeInMins
    {
        get => this.MaxRecoveryTimeInMinsDisplay ?? default;
        set => this.MaxRecoveryTimeInMinsDisplay = value;
    }
    public int RestTimeInMins
    {
        get => this.RestTimeInMinsDisplay ?? default;
        set => this.RestTimeInMinsDisplay = value;
    }
    public bool IsCompulsoryInspectionRequired
    {
        get => this.requireCompulsoryInspection;
        set => this.SetProperty(ref this.requireCompulsoryInspection, value);
    }
}
