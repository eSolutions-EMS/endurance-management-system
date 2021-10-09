using EnduranceJudge.Domain.Aggregates.Import.Models;
using EnduranceJudge.Domain.Core.Exceptions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Athletes;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Countries;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Domain.State.Horses;
using EnduranceJudge.Domain.State.Participants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using static EnduranceJudge.Localization.DesktopStrings;
using static EnduranceJudge.Localization.Strings.Domain.Countries;
using static EnduranceJudge.Domain.DomainConstants;

namespace EnduranceJudge.Domain.Aggregates.Import
{
    public class ImportManager : ManagerObjectBase, IAggregateRoot
    {
        public ImportManager(string name = EVENT_DEFAULT_NAME, Country country = null)
            => this.Validate<EnduranceEventException>(() =>
        {
            this.EnduranceEvent = new EnduranceEvent(name, country, null);
        });

        public EnduranceEvent EnduranceEvent { get; private set; }
        private readonly List<Athlete> athletes = new();
        private readonly List<Horse> horses = new();
        private readonly List<Participant> participants = new();

        public void AddCompetitions(List<HorseSportShowEntriesEvent> competitionsData)
            => this.Validate<CompetitionObjectException>(() =>
        {
            if (this.EnduranceEvent.Competitions.Any())
            {
                return;
            }

            foreach (var data in competitionsData)
            {
                var name = data.FEIID.IsRequired(NAME);
                var competition = new Competition(CompetitionType.International, name);
                this.EnduranceEvent.Add(competition);
            }
        });

        public void AddAthletes(List<HorseSportShowEntriesAthlete> athletesData)
            => this.Validate<AthleteObjectException>(() =>
        {
            if (this.athletes.Any())
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

                var athlete = new Athlete(
                    data.FEIID,
                    data.FirstName,
                    data.FamilyName,
                    data.CompetingFor,
                    birthDate);
                this.athletes.Add(athlete);
            }
        });

        public void AddHorses(List<HorseSportShowEntriesHorse> horsesData)
        {
            if (this.horses.Any())
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
                this.horses.Add(horse);
            }
        }

        public void AddParticipants(List<HorseSportShowEntriesEventAthleteEntry> participantsData)
        {
            if (this.participants.Any())
            {
                return;
            }
            if (!this.horses.Any() || !this.athletes.Any())
            {
                throw new DomainException("Cannot import Participants - empty horses and/or athletes.");
            }

            foreach (var participantData in participantsData)
            {
                var athlete = this.athletes.FirstOrDefault(x => x.FeiId == participantData.FEIID);
                var horse = this.horses.FirstOrDefault(x => x.FeiId == participantData.HorseEntry.First().FEIID);
                if (athlete != null && horse != null)
                {
                    var participant = new Participant(horse, athlete);
                    this.participants.Add(participant);
                }
            }
        }

        public void AddHorse(string name, string breed, string feiId, string club)
        {
            if (this.horses.Any())
            {
                return;
            }
            var horse = new Horse(feiId, name, breed, club);
            this.horses.Add(horse);
        }

        public void UpdateState(IState state)
        {
            if (this.EnduranceEvent != null)
            {
                state.Event = this.EnduranceEvent;
            }
            if (this.horses.Any())
            {
                state.Horses.AddRange(this.horses);
            }
            if (this.athletes.Any())
            {
                state.Athletes.AddRange(this.athletes);
            }
            if (this.participants.Any())
            {
                state.Participants.AddRange(this.participants);
            }
            state.Countries.AddRange(GetCountries());
        }

        private static List<Country> GetCountries()
        {
            var bulgaria = new Country("BUL", BULGARIA);

            return new List<Country> { bulgaria };
        }
    }
}
