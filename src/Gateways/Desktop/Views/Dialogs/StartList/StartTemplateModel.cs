using EnduranceJudge.Domain.Aggregates.Manager.Branches.StartList;
using EnduranceJudge.Localization.Translations;

namespace EnduranceJudge.Gateways.Desktop.Views.Dialogs.PrintStartList;

public class StartTemplateModel
{
    public StartTemplateModel(StartModel start)
    {
        this.Number = start.Number;
        this.Name = start.Name;
        this.CountryName = start.CountryName;
        this.Distance = start.Distance;
        this.HasStarted = start.HasStarted
            ? Words.YES
            : Words.NO;
        this.StartTimeString = start.StartTime.ToString(DesktopConstants.TIME_FORMAT);
    }

    public int Number { get; }
    public string Name { get; }
    public string CountryName { get; }
    public double Distance { get; }
    public string StartTimeString { get; }
    public string HasStarted { get; }
}
