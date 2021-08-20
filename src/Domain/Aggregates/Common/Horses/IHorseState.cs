using EnduranceJudge.Domain.Core.Models;

namespace EnduranceJudge.Domain.Aggregates.Common.Horses
{
    public interface IHorseState : IDomainModelState
    {
        string FeiId { get; }
        string Name { get; }
        bool IsStallion { get; }
        string Breed { get; }
        string TrainerFeiId { get; }
        string TrainerFirstName { get; }
        string TrainerLastName { get; }
    }
}
