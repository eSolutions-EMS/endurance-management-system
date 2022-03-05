using EnduranceJudge.Core.Utilities;
using EnduranceJudge.Domain.AggregateRoots.Import.Models;
using EnduranceJudge.Domain.AggregateRoots.Import.Models.International;
using EnduranceJudge.Domain.AggregateRoots.Import.Models.National;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Validation;
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

namespace EnduranceJudge.Domain.AggregateRoots.Import
{
    public class ImportRoot : IAggregateRoot
    {
        private readonly IState state;
        private readonly Validator<CompetitionException> validator;

        public ImportRoot()
        {
            this.state = StaticProvider.GetService<IState>();
            this.validator = new Validator<CompetitionException>();
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
        {
            if (this.state.Event.Competitions.Any())
            {
                return;
            }
            foreach (var data in competitionsData)
            {
                var name = this.validator.IsRequired(data.FEIID, NAME);
                var competition = new Competition(CompetitionType.International, name);
                this.state.Event.Save(competition);
            }
        }

        private void AddAthletes(List<HorseSportShowEntriesAthlete> athletesData)
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
        }

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
                // TODO: constant
                throw Helper.Create<ParticipantException>(
                    "Cannot import Participants - empty horses and/or athletes.");
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
