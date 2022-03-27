using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Manager;
using EnduranceJudge.Domain.AggregateRoots.Common.Performances;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Services;
using Prism.Commands;
using System;
using System.Globalization;
using System.Windows;
using static EnduranceJudge.Gateways.Desktop.DesktopConstants;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Common.Performances;

public class PerformanceTemplateModel : ViewModelBase, IMapFrom<Performance>, IPerformance
{
    private readonly IExecutor<ManagerRoot> managerExecutor;
    private readonly IDateService dateService;

    public PerformanceTemplateModel(Performance performance)
    {
        this.EditVisibility = Visibility.Visible;
        this.managerExecutor = StaticProvider.GetService<IExecutor<ManagerRoot>>();
        this.dateService = StaticProvider.GetService<IDateService>();
        this.Edit = new DelegateCommand(this.EditAction);
        this.HeaderValue = $"{GATE.ToUpper()}{performance.Index}/{this.TotalLength} {KM}";
        this.Update(performance);
    }

    public Visibility EditVisibility { get; set; }
    public Visibility ReadonlyVisibility => this.EditVisibility == Visibility.Collapsed
        ? Visibility.Visible
        : Visibility.Collapsed;

    public DelegateCommand Edit { get; }

    public string HeaderValue { get; }
    private DateTime startTime;
    private string arrivalTimeString;
    private string inspectionTimeString;
    private string reInspectionTimeString;
    private string requiredInspectionTimeString;
    private string compulsoryRequiredInspectionTimeString;
    private string recoverySpanString;
    private string timeString;
    public string nextStartTimeString;
    private string averageSpeedForLoopKpHString;
    private string averageSpeedTotalKpHString;

    public void EditAction()
    {
        var result = this.managerExecutor.Execute(x => x.EditRecord(this));
        this.Update(result);
    }

    public void Update(Performance performance)
    {
        // TODO: remove AutoMapper
        this.MapFrom(performance);
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
    public int Index { get; }
    public DateTime? RequiredInspectionTime
    {
        get => this.ParseTime(this.RequiredInspectionTimeString);
        private set => this.RequiredInspectionTimeString = this.FormatTime(value);
    }
    public DateTime? CompulsoryRequiredInspectionTime // Possibly remove
    {
        get => this.ParseTime(this.CompulsoryRequiredInspectionTimeString);
        private set => this.CompulsoryRequiredInspectionTimeString = this.FormatTime(value);
    }
    public TimeSpan? RecoverySpan
    {
        get => this.ParseSpan(this.RecoverySpanString);
        private set => this.RecoverySpanString = this.FormatSpan(value);
    }
    public TimeSpan? Time
    {
        get => this.ParseSpan(this.TimeString);
        private set => this.TimeString = this.FormatSpan(value);
    }
    public double? AverageSpeed
    {
        get => double.Parse(this.AverageSpeedString);
        private set => this.AverageSpeedString = value?.ToString("#.###");
    }
    public double? AverageSpeedTotal
    {
        get => double.Parse(this.AverageSpeedTotalString);
        private set => this.AverageSpeedTotalString = value?.ToString("#.###");
    }
    public DateTime? NextStartTime
    {
        get => this.ParseTime(this.NextStartTimeString);
        private set => this.NextStartTimeString = this.FormatTime(value);
    }
    public bool IsReInspectionRequired { get; private set; }
    public bool IsRequiredInspectionRequired { get; private set; }
    public double TotalLength { get; private set;  }
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
    private TimeSpan? ParseSpan(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return null;
        }
        var span = TimeSpan.Parse(value);
        return span;
    }
    private string FormatSpan(TimeSpan? span)
    {
        var spanString = span?.ToString(TIME_SPAN_FORMAT);
        return spanString;
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
    public string RecoverySpanString
    {
        get => this.recoverySpanString;
        private set => this.SetProperty(ref this.recoverySpanString, value);
    }
    public string TimeString
    {
        get => this.timeString;
        private set => this.SetProperty(ref this.timeString, value);
    }
    public string AverageSpeedString
    {
        get => this.averageSpeedForLoopKpHString;
        private set => this.SetProperty(ref this.averageSpeedForLoopKpHString, value);
    }
    public string AverageSpeedTotalString
    {
        get => this.averageSpeedTotalKpHString;
        private set => this.SetProperty(ref this.averageSpeedTotalKpHString, value);
    }
    public string NextStartTimeString
    {
        get => this.nextStartTimeString;
        private set => this.SetProperty(ref this.nextStartTimeString, value);
    }

#endregion Setters;
}
