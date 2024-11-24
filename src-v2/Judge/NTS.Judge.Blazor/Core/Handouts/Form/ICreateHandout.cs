using NTS.Domain.Core.Entities.ParticipationAggregate;

namespace NTS.Judge.Blazor.Core.Handouts.Form;
public interface ICreateHandout
{
    Task Create(int number);
    Task<IEnumerable<Combination>> GetCombinations();
}
