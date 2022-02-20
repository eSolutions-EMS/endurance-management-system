using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.Aggregates.Manager;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.PhasePerformances;

public class PerformanceTemplateModel : ViewModelBase, IMapFrom<Performance>, IPerformanceState
{
    private readonly IExecutor<ContestManager> competitionExecutor;
    private readonly IQueries<Performance> performances;
    private readonly IDateService dateService;

    public PerformanceTemplateModel()
    {
        this.competitionExecutor = StaticProvider.GetService<IExecutor<ContestManager>>();
        this.performances = StaticProvider.GetService<IQueries<Performance>>();
        this.dateService = StaticProvider.GetService<IDateService>();
        this.Edit = new DelegateCommand(this.EditAction);
    }

    public DelegateCommand Edit { get; }

    private DateTime startTime;
    private string arrivalTimeString;
    private string inspectionTimeString;
    private string reInspectionTimeString;
    private string requiredInspectionTimeString;
    private bool isAnotherInspectionRequired;
    private int phaseLengthInKm;
    private TimeSpan loopSpan;
    private TimeSpan phaseSpan;
    private double? averageSpeedForLoopInKpH;
    public double? averageSpeedForPhaseInKpH;
    public DateTime? nextPerformanceStartTime;
    public bool isComplete;

    public void EditAction()
    {
        this.competitionExecutor.Execute(x => x.EditPerformance(this));
        var result = this.performances.GetOne(this.Id);
        this.MapFrom(result);
    }

    #region IPerformanceState implementation

    public DateTime? ArrivalTime
    {
        get => this.ParseTime(this.ArrivalTimeString);
        private set => this.ArrivalTimeString = this.FormatTime(value);
    }
    public DateTime? InspectionTime
    {
        get => this.ParseTime(this.InspectionTimeString);
        private set => this.InspectionTimeString = this.FormatTime(value);
    }
    public DateTime? ReInspectionTime
    {
        get => this.ParseTime(this.ReInspectionTimeString);
        private set => this.ReInspectionTimeString = this.FormatTime(value);
    }
    public DateTime? RequiredInspectionTime
    {
        get => this.ParseTime(this.RequiredInspectionTimeString);
        private set => this.RequiredInspectionTimeString = this.FormatTime(value);
    }

    public int Id { get; private set; }

    #endregion

    private DateTime? ParseTime(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        var date = this.dateService.Parse(value, TIME_FORMAT);
        return date;
    }
    private string FormatTime(DateTime? time)
    {
        var timeString = time?.ToString(TIME_FORMAT);
        return timeString;
    }

    #region Setters

    public DateTime StartTime
    {
        get => this.startTime;
        private set => this.SetProperty(ref this.startTime, value);
    }
    public string ArrivalTimeString
    {
        get => this.arrivalTimeString;
        set => this.SetProperty(ref this.arrivalTimeString, value);
    }
    public string InspectionTimeString
    {
        get => this.inspectionTimeString;
        set => this.SetProperty(ref this.inspectionTimeString, value);
    }
    public string ReInspectionTimeString
    {
        get => this.reInspectionTimeString;
        set => this.SetProperty(ref this.reInspectionTimeString, value);
    }
    public string RequiredInspectionTimeString
    {
        get => this.requiredInspectionTimeString;
        set => this.SetProperty(ref this.requiredInspectionTimeString, value);
    }
    public bool IsRequiredInspectionRequired
    {
        get => this.isAnotherInspectionRequired;
        private set => this.SetProperty(ref this.isAnotherInspectionRequired, value);
    }
    public int PhaseLengthInKm
    {
        get => this.phaseLengthInKm;
        private set => this.SetProperty(ref this.phaseLengthInKm, value);
    }

    public TimeSpan LoopSpan
    {
        get => this.loopSpan;
        private set => this.SetProperty(ref this.loopSpan, value);
    }
    public TimeSpan PhaseSpan
    {
        get => this.phaseSpan;
        private set => this.SetProperty(ref this.phaseSpan, value);
    }
    public double? AverageSpeedForLoopInKpH
    {
        get => this.averageSpeedForLoopInKpH;
        private set => this.SetProperty(ref this.averageSpeedForLoopInKpH, value);
    }
    public double? AverageSpeedForPhaseInKpH
    {
        get => this.averageSpeedForPhaseInKpH;
        private set => this.SetProperty(ref this.averageSpeedForPhaseInKpH, value);
    }
    public bool IsComplete
    {
        get => this.isComplete;
        private set => this.SetProperty(ref this.isComplete, value);
    }
    public DateTime? NextPerformanceStartTime
    {
        get => this.nextPerformanceStartTime;
        private set => this.SetProperty(ref this.nextPerformanceStartTime, value);
    }

    #endregion Setters;
}
