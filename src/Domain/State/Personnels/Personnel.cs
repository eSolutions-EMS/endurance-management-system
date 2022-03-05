using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Domain.State.Personnels
{
    public class Personnel : DomainObjectBase<PersonnelException>, IPersonnelState
    {
        private Personnel() {}
        public Personnel(IPersonnelState state) : base(GENERATE_ID)
        {
            this.Name = this.Validator.IsFullName(state.Name);
            this.Role = this.Validator.IsRequired(state.Role, nameof(state.Role));
        }

        public string Name { get; private set; }
        public PersonnelRole Role { get; private set; }
    }
}
