using EnduranceJudge.Application.Factories;
using EnduranceJudge.Application.Import.Factories;
using EnduranceJudge.Application.Import.ImportFromFile.Models;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Domain.Aggregates.Import.Competitions;
using EnduranceJudge.Domain.Aggregates.Import.EnduranceEvents;
using EnduranceJudge.Domain.Aggregates.Import.Participants;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Application.Import.ImportFromFile.Services.Implementations
{
    public class InternationalImportService : IInternationalImportService
    {
        private readonly IXmlSerializationService xmlSerialization;
        private readonly ICompetitionFactory competitionFactory;
        private readonly IParticipantFactory participantFactory;
        private readonly IAthleteFactory athleteFactory;
        private readonly IHorseFactory horseFactory;

        public InternationalImportService(
            IXmlSerializationService xmlSerialization,
            ICompetitionFactory competitionFactory,
            IParticipantFactory participantFactory,
            IAthleteFactory athleteFactory,
            IHorseFactory horseFactory)
        {
            this.xmlSerialization = xmlSerialization;
            this.competitionFactory = competitionFactory;
            this.participantFactory = participantFactory;
            this.athleteFactory = athleteFactory;
            this.horseFactory = horseFactory;
        }

        public EnduranceEvent FromInternational(string filePath)
        {
            var importData = this.xmlSerialization.Deserialize<HorseSport>(filePath);
            var showEntries = importData?.Items?.FirstOrDefault() as HorseSportShowEntries;
            if (showEntries == null)
            {
                return null;
            }

            var athleteData = showEntries.Athlete.ToList();
            var horseData = showEntries.Horse.ToList();
            var eventData = showEntries.Event.ToList();
            var athletes = athleteData
                .Select(this.athleteFactory.Create)
                .ToList();
            var horses = horseData
                .Select(this.horseFactory.Create)
                .ToList();

            var competitions = new List<Competition>();
            foreach (var data in eventData)
            {
                var entries = data.AthleteEntry.ToList();
                var participants = new List<Participant>();
                foreach (var entry in entries)
                {
                    var athlete = athletes.FirstOrDefault(x => x.FeiId == entry.FEIID);
                    var horse = horses.FirstOrDefault(x => x.FeiId == entry.HorseEntry.FirstOrDefault()?.FEIID);
                    var participant = this.participantFactory.Create(athlete, horse);
                    participants.Add(participant);
                }
                var competition = this.competitionFactory.Create(data.FEIID, participants);
                competitions.Add(competition);
            }

            var showData = showEntries.Show.FirstOrDefault();
            var venue = showData?.Venue.FirstOrDefault();
            if (venue == null)
            {
                return new EnduranceEvent(competitions);
            }
            return new EnduranceEvent(competitions, venue.Name, venue.CountryNOC);
        }
    }
}
