using EnduranceJudge.Domain.Aggregates.Rankings.Competitions;
using EnduranceJudge.Domain.Aggregates.Rankings.Stateless.Classifications;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Rankings.Stateless.Rankings
{
    public class Ranking : DomainBase<RankingException>
    {
        private readonly List<Classification> classifications = new();

        public Ranking(IEnumerable<Competition> competitions)
        {
            foreach (var competition in competitions)
            {
                var kidsClassification = new Classification(Category.Kids, competition);
                var adultsClassification = new Classification(Category.Adults, competition);

                if (kidsClassification.RankList.Any())
                {
                    this.classifications.Add(kidsClassification);
                }
                if (adultsClassification.RankList.Any())
                {
                    this.classifications.Add(adultsClassification);
                }
            }
        }

        public IReadOnlyList<Classification> Classifications => this.classifications.AsReadOnly();
    }
}
