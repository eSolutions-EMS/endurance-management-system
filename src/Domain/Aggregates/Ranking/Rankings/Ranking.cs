using EnduranceJudge.Domain.Core;
using EnduranceJudge.Domain.Aggregates.Ranking.Classifications;
using EnduranceJudge.Domain.Aggregates.Ranking.Competitions;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using System.Collections.Generic;

namespace EnduranceJudge.Domain.Aggregates.Ranking.Rankings
{
    public class Ranking : DomainBase<RankingException>, IAggregateRoot
    {
        private readonly List<Classification> classifications = new();

        public Ranking(IReadOnlyCollection<Competition> competitions)
        {
            foreach (var competition in competitions)
            {
                var kidsClassification = new Classification(Category.Kids, competition);
                var adultsClassification = new Classification(Category.Adults, competition);

                this.classifications.Add(kidsClassification);
                this.classifications.Add(adultsClassification);
            }
        }

        public IReadOnlyList<Classification> Classifications => this.classifications.AsReadOnly();
    }
}
