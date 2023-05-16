using EnduranceJudge.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using static EnduranceJudge.Localization.Strings;

namespace EnduranceJudge.Gateways.Desktop.Views.Dialogs.Startlists;

public class StartTemplateModel
{
    public StartTemplateModel(StartModel start)
    {
        this.Name = start.Name;
        this.CountryName = start.CountryName;
        this.Distance = start.Distance;
        this.StartTimeString = start.StartTime.ToString(DesktopConstants.TIME_FORMAT);
    }

    public string Name { get; }
    public string CountryName { get; }
    public double Distance { get; }
    public string StartTimeString { get; }
}
