using EnduranceJudge.Domain.AggregateRoots.Rankings.Aggregates;
using EnduranceJudge.Domain.Core.Extensions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.AggregateRoots.Rankings;

public class RankingRoot : IAggregate, IAggregateRoot
{
    private readonly List<CompetitionResultAggregate> competitions = new();

    public RankingRoot(IState state)
    {
        // Program.Main();
        if (state.Event == default)
        {
            return;
        }
        var competitionsIds = state.Participations
            .SelectMany(x => x.CompetitionsIds)
            .Distinct()
            .ToList();
        foreach (var id in competitionsIds)
        {
            var competition = state.Event.Competitions.FindDomain(id);
            var participations = state.Participations
                .Where(x => x.CompetitionsIds.Contains(competition.Id))
                .ToList();
            var listing = new CompetitionResultAggregate(state.Event, competition, participations);
            this.competitions.Add(listing);
        }
    }

    public IReadOnlyList<CompetitionResultAggregate> Competitions => this.competitions.AsReadOnly();
}
// TODO: remove
//
//     public class Kur : IEqualityComparer<Competition>
//     {
//         public bool Equals(Competition x, Competition y)
//         {
//             if (ReferenceEquals(x, null))
//             {
//                 return ReferenceEquals(y, null);
//             }
//             var debel = x.Equals(y);
//             return debel;
//         }
//         public int GetHashCode(Competition obj)
//         {
//             return HashCode.Combine((int)obj.Type, obj.Name, obj.StartTime);
//         }
//     }
//
//     public class Program
//     {
//         public static void Main()
//         {
//             var a = new Test { Id = 1 };
//             var a2 = new Test { Id = 1 };
//             var list = new List<Test> { a, a2 };
//
//             var distinct = list.Distinct().ToList(); // all items, Equal implementations not called
//             var containsA = list.Contains(a); // true, Equal implementations called
//             var containsA2 = list.Contains(a); // true
//             var containsNewObjectWithSameId = list.Contains(new Test { Id = 1 }); // true
//             ;
//         }
//     }
//
// public class Test : IEquatable<Test>
// {
//     public int Id { get; init; }
//     public bool Equals(Test other)
//     {
//         if (ReferenceEquals(null, other))
//             return false;
//         if (ReferenceEquals(this, other))
//             return true;
//         if (this.GetType() != other.GetType())
//             return false;
//         return this.Id == other.Id;
//     }
//
//     public override int GetHashCode() => base.GetHashCode() + this.Id;
// }