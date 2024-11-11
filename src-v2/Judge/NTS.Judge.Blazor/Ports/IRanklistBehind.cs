using Not.Blazor.Observable.Ports;
using Not.Injection;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Ports;

public interface IRanklistBehind : IObservableBehind, ISingleton
{
    Ranklist? Ranklist { get; }
    Task<IEnumerable<Ranking>> GetRankings();
    Task SelectRanking(int id);
}
