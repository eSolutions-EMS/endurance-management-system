using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using EnduranceJudge.Domain.State.Personnels;
using EnduranceJudge.Domain.States;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Localization.DesktopStrings;

namespace EnduranceJudge.Domain.State
{
    public class EventState : DomainObjectBase<EventConfigurationException>, IEventState, IAggregateRoot
    {
        private EventState()  {}
        public EventState(int id, string name, string populatedPlace, string countryCode)
            : base(id) => this.Validate(() =>
        {
            this.Name = name.IsRequired(NAME);
            this.PopulatedPlace = populatedPlace.IsRequired(POPULATED_PLACE);
            this.CountryIsoCode = countryCode.IsRequired(COUNTRY);
        });

        private List<Personnel> membersOfVetCommittee = new();
        private List<Personnel> membersOfJudgeCommittee = new();
        private List<Personnel> stewards = new();
        private List<Competition> competitions = new();
        private List<Participant> participants = new();

        public string Name { get; private set; }
        public string PopulatedPlace { get; private set; }
        public string CountryIsoCode { get; private set; }
        public Personnel PresidentGroundJury { get; private set; }
        public Personnel PresidentVetCommission { get; private set; }
        public Personnel ForeignJudge { get; private set; }
        public Personnel FeiTechDelegate { get; private set; }
        public Personnel FeiVetDelegate { get; private set; }
        public Personnel ActiveVet { get; private set; }

        public void Add(Competition competition) => this.Validate(() =>
        {
            this.competitions.AddOrUpdateObject(competition);
        });
        public void Add(Participant participant) => this.Validate(() =>
        {
            this.participants.AddOrUpdateObject(participant);
        });
        public void Add(Personnel personnel) => this.Validate(() =>
        {
            switch (personnel.Role)
            {
                case PersonnelRole.PresidentGroundJury:
                    this.PresidentGroundJury = personnel;
                    break;
                case PersonnelRole.PresidentVetCommission:
                    this.PresidentVetCommission = personnel;
                    break;
                case PersonnelRole.ForeignJudge:
                    this.ForeignJudge = personnel;
                    break;
                case PersonnelRole.FeiTechDelegate:
                    this.FeiTechDelegate = personnel;
                    break;
                case PersonnelRole.FeiVetDelegate:
                    this.FeiVetDelegate = personnel;
                    break;
                case PersonnelRole.ActiveVet:
                    this.ActiveVet = personnel;
                    break;
                case PersonnelRole.MemberOfVetCommittee:
                    this.membersOfVetCommittee.Add(personnel);
                    break;
                case PersonnelRole.MemberOfJudgeCommittee:
                    this.membersOfJudgeCommittee.Add(personnel);
                    break;
                case PersonnelRole.Steward:
                    this.stewards.Add(personnel);
                    break;
            }
        });

        public IReadOnlyList<Personnel> MembersOfVetCommittee
        {
            get => this.membersOfVetCommittee.AsReadOnly();
            private set => this.membersOfVetCommittee = value.ToList();
        }
        public IReadOnlyList<Personnel> MembersOfJudgeCommittee
        {
            get => this.membersOfJudgeCommittee.AsReadOnly();
            private set => this.membersOfJudgeCommittee = value.ToList();
        }
        public IReadOnlyList<Personnel> Stewards
        {
            get => this.stewards.AsReadOnly();
            private set => this.stewards = value.ToList();
        }
        public IReadOnlyList<Competition> Competitions
        {
            get => this.competitions.AsReadOnly();
            private set => this.competitions = value.ToList();
        }
        public IReadOnlyList<Participant> Participants
        {
            get => this.participants.AsReadOnly();
            private set => this.participants = value.ToList();
        }
    }
}
