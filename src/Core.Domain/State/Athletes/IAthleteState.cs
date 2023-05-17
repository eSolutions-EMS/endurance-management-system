using Core.Domain.Enums;
using Core.Models;

namespace Core.Domain.State.Athletes;

public interface IAthleteState : IIdentifiable
{
    public string FeiId { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Club { get; }
    public Category Category { get; }
}
