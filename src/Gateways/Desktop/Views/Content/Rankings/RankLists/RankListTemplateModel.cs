using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates;
using EnduranceJudge.Domain.Aggregates.Rankings.AggregateBranches;
using EnduranceJudge.Gateways.Desktop.Core;
using EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.RankListEntry;
using System;
using System.Collections.ObjectModel;
using static EnduranceJudge.Localization.Translations.Words;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Rankings.RankLists;

public class RankListTemplateModel : ViewModelBase, ICompetitionData
{
    public const string DATE_TIME_FORMAT = "dd.mm.yyyy / HH:mm:ss";
    public const string DATE_ONLY_FORMAT = "dd.mm.yyyy";

    public RankListTemplateModel(CompetitionResult result)
    {
        this.MapFrom<ICompetitionData>(result);
    }

    public ObservableCollection<RankListEntryTemplateModel> RankList { get; }

    public string Title => $"{this.CompetitionName} * {this.CompetitionLengthInKm} {KM.ToUpper()}";
    public string EventName { get; }
    public string PopulatedPlace { get; }
    public string CountryName { get; }
    public DateTime CompetitionDate { get; }
    public string Organizer { get; }
    public string CompetitionName { get; }
    public double CompetitionLengthInKm { get; }
    public string PresidentGroundJuryName { get; }
    public string ChiefStewardName { get; }
    public DateTime DateNow { get; }

    public string CompetitionDateString => this.CompetitionDate.ToString(DATE_ONLY_FORMAT);
    public string DateNowString => this.DateNow.ToString(DATE_TIME_FORMAT);
}
