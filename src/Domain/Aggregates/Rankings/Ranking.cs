using EnduranceJudge.Domain.Aggregates.Rankings.AggregateBranches;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.State;
using EnduranceJudge.Domain.State.Competitions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Rankings
{
    public class Ranking : ManagerObjectBase, IAggregateRoot
    {
        private readonly List<CompetitionResult> competitions = new();

        public Ranking(IState state)
        {
            // Program.Main();
            if (state.Event == default)
            {
                return;
            }
            var participants = state.Participants;
            var competitions = participants
                .Select(x => x.Participation)
                .SelectMany(x => x.Competitions)
                .Distinct()
                .ToList();
            foreach (var competition in competitions)
            {
                var participantsInCompetition = participants
                    .Where(x => x.Participation.Competitions.Contains(competition))
                    .ToList();
                var listing = new CompetitionResult(state.Event, competition, participantsInCompetition);
                this.competitions.Add(listing);
            }
        }

        public IReadOnlyList<CompetitionResult> Competitions => this.competitions.AsReadOnly();
    }
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
}
