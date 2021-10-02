using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Aggregates.State.Competitions;
using EnduranceJudge.Domain.Aggregates.State.Personnels;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.State
{
    public class EventState : DomainObjectBase<EventConfigurationException>, IEventState, IAggregateRoot
    {
        public EventState(IEventState state) : base(state.Id) => this.Validate(() =>
        {
            this.Name = state.Name.IsRequired(nameof(state.Name));
            this.PopulatedPlace = state.PopulatedPlace.IsRequired(nameof(state.PopulatedPlace));
            this.CountryIsoCode = state.CountryIsoCode.IsRequired(nameof(state.CountryIsoCode));
        });

        private List<Personnel> membersOfVetCommittee = new();
        private List<Personnel> membersOfJudgeCommittee = new();
        private List<Personnel> stewards = new();
        private List<Competition> competitions = new();
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
    }
}
