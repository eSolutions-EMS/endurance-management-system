using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.Aggregates.Import.Models;
using EnduranceJudge.Domain.Aggregates.Import.Models.International;
using EnduranceJudge.Domain.Aggregates.Import.Models.National;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static EnduranceJudge.Localization.Translations.Words;
using static EnduranceJudge.Domain.DomainConstants;

namespace EnduranceJudge.Domain.Aggregates.Import
{
    public class ImportManager : ManagerObjectBase, IAggregateRoot
    {
        private readonly IState state;

        public ImportManager()
        {
            this.state = StaticProvider.GetService<IState>();
        }

        public void Import(InternationalData data)
        {
            var eventName = data.Event?.Name ?? EVENT_DEFAULT_NAME;
            var country = this.state.Countries.FirstOrDefault(x => x.IsoCode == data.Event.CountryNOC);
            this.state.Event = new EnduranceEvent(eventName, country);

            this.AddCompetitions(data.Competitions);
            this.AddAthletes(data.Athletes);
            this.AddHorses(data.Horses);
            this.AddParticipants(data.Participants);
        }

        public void Import(NationalData data)
        {
            this.state.Event ??= new EnduranceEvent(null, null);
            foreach (var horseData in data.Horses)
            {
                var horse = new Horse(horseData.FeiId, horseData.Name, horseData.Breed, horseData.Club);
                this.state.Horses.Add(horse);
            }
        }

        private void AddCompetitions(List<HorseSportShowEntriesEvent> competitionsData)
            => this.Validate<CompetitionException>(() =>
        {
            if (this.state.Event.Competitions.Any())
            {
                return;
            }

            foreach (var data in competitionsData)
            {
                var name = data.FEIID.IsRequired(NAME);
                var competition = new Competition(CompetitionType.International, name);
                this.state.Event.Save(competition);
            }
        });

        private void AddAthletes(List<HorseSportShowEntriesAthlete> athletesData)
            => this.Validate<AthleteException>(() =>
        {
            if (this.state.Athletes.Any())
            {
                return;
            }

            foreach (var data in athletesData)
            {
                var hasParsed = DateTime.TryParseExact(
                    data.BirthDate,
                    "yyyy-mm-dd",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out var birthDate);
                if (!hasParsed)
                {
                    continue;
                }

                var country = this.state.Countries.FirstOrDefault(x => x.IsoCode == data.CompetingFor);
                var athlete = new Athlete(data.FEIID, data.FirstName, data.FamilyName, country, birthDate);
                this.state.Athletes.Add(athlete);
            }
        });

        private void AddHorses(List<HorseSportShowEntriesHorse> horsesData)
        {
            if (this.state.Horses.Any())
            {
                return;
            }

            foreach (var data in horsesData)
            {
                var hasParsed = bool.TryParse(data.Stallion, out var isStallion);
                if (!hasParsed)
                {
                    continue;
                }

                var horse = new Horse(
                    data.FEIID,
                    data.Name,
                    isStallion,
                    data.StudBook,
                    data.TrainerFEIID,
                    data.TrainerFirstName,
                    data.TrainerFamilyName);
                this.state.Horses.Add(horse);
            }
        }

        private void AddParticipants(List<HorseSportShowEntriesEventAthleteEntry> participantsData)
        {
            if (this.state.Participants.Any())
            {
                return;
            }
            if (!this.state.Horses.Any() || !this.state.Athletes.Any())
            {
                throw new DomainException("Cannot import Participants - empty horses and/or athletes.");
            }

            foreach (var participantData in participantsData)
            {
                var athlete = this.state.Athletes.FirstOrDefault(x => x.FeiId == participantData.FEIID);
                var horse = this.state.Horses.FirstOrDefault(x => x.FeiId == participantData.HorseEntry.First().FEIID);
                if (athlete != null && horse != null)
                {
                    var participant = new Participant(athlete, horse);
                    this.state.Participants.Add(participant);
                }
            }
        }
    }
}
