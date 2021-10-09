using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Validation;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.States;

namespace EnduranceJudge.Domain.State.Personnels
{
    public class Personnel : DomainObjectBase<PersonnelException>, IPersonnelState
    {
        private Personnel() {}
        public Personnel(string name, PersonnelRole role) : base(true)
            => this.Validate(() =>
            {
                this.Name = name.CheckPersonName();
                this.Role = role.IsNotDefault(nameof(role));
            });

        public string Name { get; private set; }
        public PersonnelRole Role { get; private set; }
    }
}
