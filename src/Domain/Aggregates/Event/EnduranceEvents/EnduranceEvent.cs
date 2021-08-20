using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Domain.Aggregates.Common.Countries;
using EnduranceJudge.Domain.Aggregates.Event.Competitions;
using EnduranceJudge.Domain.Aggregates.Event.Personnels;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents
{
    public class EnduranceEvent : DomainBase<EnduranceEventException>, IEnduranceEventState, IAggregateRoot
    {
        private EnduranceEvent()
        {
        }

        public EnduranceEvent(IEnduranceEventState state) : base(state.Id)
            => this.Validate(() =>
            {
                this.Name = state.Name.IsRequired(nameof(state.Name));
                this.PopulatedPlace = state.PopulatedPlace.IsRequired(nameof(state.PopulatedPlace));
                this.CountryIsoCode = state.CountryIsoCode.IsRequired(nameof(state.CountryIsoCode));
            });

        public string Name { get; private set; }
        public string PopulatedPlace { get; private set; }

        public string CountryIsoCode { get; private set; }

        public Personnel PresidentGroundJury
            => this.personnel.FirstOrDefault(p => p.Role == PersonnelRole.PresidentGroundJury);
        public Personnel PresidentVetCommission
            => this.personnel.FirstOrDefault(p => p.Role == PersonnelRole.PresidentVetCommission);
        public Personnel ForeignJudge
            => this.personnel.FirstOrDefault(p => p.Role == PersonnelRole.ForeignJudge);
        public Personnel FeiTechDelegate
            => this.personnel.FirstOrDefault(p => p.Role == PersonnelRole.FeiTechDelegate);
        public Personnel FeiVetDelegate
            => this.personnel.FirstOrDefault(p => p.Role == PersonnelRole.FeiVetDelegate);
        public Personnel ActiveVet
            => this.personnel.FirstOrDefault(p => p.Role == PersonnelRole.ActiveVet);
        public IReadOnlyList<Personnel> MembersOfVetCommittee
            => this.personnel
                .Where(p => p.Role == PersonnelRole.MemberOfVetCommittee)
                .ToList()
                .AsReadOnly();
        public IReadOnlyList<Personnel> MembersOfJudgeCommittee
            => this.personnel
                .Where(p => p.Role == PersonnelRole.MemberOfJudgeCommittee)
                .ToList()
                .AsReadOnly();
        public IReadOnlyList<Personnel> Stewards
            => this.personnel
                .Where(p => p.Role == PersonnelRole.Steward)
                .ToList()
                .AsReadOnly();

        private List<Personnel> personnel = new();
        public IReadOnlyList<Personnel> Personnel
        {
            get => this.personnel.AsReadOnly();
            private set => this.personnel = value.ToList();
        }

        public EnduranceEvent Add(IEnumerable<Personnel> personnel)
        {
            personnel.ForEach(this.personnel.Add);
            return this;
        }
        public EnduranceEvent Add(Personnel personnel)
        {
            var areRoleDuplicatesAllowed = IsRoleMultiPersonnel(personnel.Role);

            if (areRoleDuplicatesAllowed && this.personnel.Any(p => p.Name == personnel.Name))
            {
                // TODO: Extract in Localized text
                var message = $"Cannot add {personnel.Role}' - name '{personnel.Name}' already exits";
                this.Throw(message);
            }

            if (!areRoleDuplicatesAllowed && this.personnel.Any(p => p.Role == personnel.Role))
            {
                var message = $"Cannot add {personnel.Role} - it already exits";
                this.Throw(message);
            }

            this.personnel.AddObject(personnel);
            return this;
        }

        private List<Competition> competitions = new();
        public IReadOnlyList<Competition> Competitions
        {
            get => this.competitions.AsReadOnly();
            private set => this.competitions = value.ToList();
        }
        public EnduranceEvent Add(Competition competition)
        {
            this.competitions.AddOrUpdateObject(competition);
            return this;
        }
        public EnduranceEvent Remove(Competition competition)
        {
            this.competitions.RemoveObject(competition);
            return this;
        }

        public static bool IsRoleMultiPersonnel(PersonnelRole role)
        {
            var isMultiPersonnel = role
                is PersonnelRole.Steward
                or PersonnelRole.MemberOfVetCommittee
                or PersonnelRole.MemberOfJudgeCommittee;

            return isMultiPersonnel;
        }
    }
}
