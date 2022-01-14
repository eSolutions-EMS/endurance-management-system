using EnduranceJudge.Domain.Core;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using EnduranceJudge.Domain.State.Competitions;
using EnduranceJudge.Domain.State.Participants;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Rankings.AggregateBranches
{
    public class CompetitionResult : ManagerObjectBase
    {
        internal CompetitionResult(Competition competition, IList<Participant> participants)
        {
            var kidsRankList = new RankList(Category.Kids, participants);
            var adultsRankList = new RankList(Category.Adults, participants);
            this.Length = competition.Phases.Aggregate(0d, (total, x) => total + x.LengthInKm);
            this.Name = competition.Name;

            if (kidsRankList.Any())
            {
                this.KidsRankList = kidsRankList;
            }
            if (adultsRankList.Any())
            {
                this.AdultsRankList = adultsRankList;
            }

            this.Id = DomainIdProvider.Generate();
        }

        public int Id { get; }
        public string Name { get; }
        public double Length { get; }
        public RankList KidsRankList { get; }
        public RankList AdultsRankList { get; }
    }
}
