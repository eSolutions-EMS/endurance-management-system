using EnduranceJudge.Application.Aggregates.Configurations.Contracts;
using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.State.Performances;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Services;
using EnduranceJudge.Localization.Strings;
using Prism.Commands;
using System;
using System.Windows;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;
using static EnduranceJudge.Localization.Strings.Words;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Performances;

public class PerformanceTemplateModel : ViewModelBase, IMapFrom<Performance>, IPerformanceState
{
    private readonly IExecutor<ManagerRoot> competitionExecutor;
    private readonly IQueries<Performance> performances;
    private readonly IDateService dateService;

    public PerformanceTemplateModel(Performance performance, int index, bool allowEdit)
    {
        this.EditVisibility = allowEdit
            ? Visibility.Visible
            : Visibility.Collapsed;
        this.ReadonlyVisibility = allowEdit
            ? Visibility.Collapsed
            : Visibility.Visible;
        this.competitionExecutor = StaticProvider.GetService<IExecutor<ManagerRoot>>();
        this.performances = StaticProvider.GetService<IQueries<Performance>>();
        this.dateService = StaticProvider.GetService<IDateService>();
        this.Edit = new DelegateCommand(this.EditAction);
        this.MapFrom(performance);
        this.HeaderValue = $"{GATE.ToUpper()}{index}/{this.LengthSoFar} {Words.KM}";
    }

    public Visibility EditVisibility { get; }
    public Visibility ReadonlyVisibility { get; }
    public DelegateCommand Edit { get; }

    public string HeaderValue { get; }
    private DateTime startTime;
    private string arrivalTimeString;
    private string inspectionTimeString;
    private string reInspectionTimeString;
    private string requiredInspectionTimeString;
    private string compulsoryRequiredInspectionTimeString;
    private TimeSpan? recoverySpan;
    private TimeSpan? time;
    private double? averageSpeedForLoopKpH;
    private double? averageSpeedTotalKpH;
    public DateTime? nextPerformanceStartTime;

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
    public DateTime? CompulsoryRequiredInspectionTime
    {
        get => this.ParseTime(this.CompulsoryRequiredInspectionTimeString);
        private set => this.CompulsoryRequiredInspectionTimeString = this.FormatTime(value);
    }
    public bool IsRequiredInspectionRequired { get; private set; }
    public double LengthSoFar { get; private set;  }
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
    public string CompulsoryRequiredInspectionTimeString
    {
        get => this.compulsoryRequiredInspectionTimeString;
        set => this.SetProperty(ref this.compulsoryRequiredInspectionTimeString, value);
    }
    public TimeSpan? RecoverySpan
    {
        get => this.recoverySpan;
        private set => this.SetProperty(ref this.recoverySpan, value);
    }
    public TimeSpan? Time
    {
        get => this.time;
        private set => this.SetProperty(ref this.time, value);
    }
    public double? AverageSpeed
    {
        get => this.averageSpeedForLoopKpH;
        private set => this.SetProperty(ref this.averageSpeedForLoopKpH, value);
    }
    public double? AverageSpeedTotal
    {
        get => this.averageSpeedTotalKpH;
        private set => this.SetProperty(ref this.averageSpeedTotalKpH, value);
    }
    public DateTime? NextPerformanceStartTime
    {
        get => this.nextPerformanceStartTime;
        private set => this.SetProperty(ref this.nextPerformanceStartTime, value);
    }

    #endregion Setters;
}
