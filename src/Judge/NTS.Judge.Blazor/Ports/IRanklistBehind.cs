using Not.Blazor.Ports.Behinds;
using Not.Injection;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
 
namespace NTS.Judge.Blazor.Ports;

public interface IRanklistBehind : IObservableBehind, ISingletonService
{
    Ranklist? Ranklist { get; }
    Task<IEnumerable<Ranking>> GetRankings();
    Task SelectRanking(int id);
}
