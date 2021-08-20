using EnduranceJudge.Domain.Core.Validation;
using EnduranceJudge.Domain.Aggregates.Ranking.Competitions;
using EnduranceJudge.Domain.Aggregates.Ranking.Participations;
using EnduranceJudge.Domain.Core.Models;
using EnduranceJudge.Domain.Enums;
using System.Collections.Generic;
using System.Linq;

namespace EnduranceJudge.Domain.Aggregates.Ranking.Classifications
{
    public class Classification : DomainBase<ClassificationException>
    {
        internal Classification(Category category, Competition competition)
        {
            this.Validate(() =>
            {
                this.Category = category.IsRequired(nameof(category));
            });

            this.LengthInKilometers = competition.LengthInKilometers;
            this.RankList = this.Classify(competition.Participations);
        }

        public int LengthInKilometers { get; }
        public Category Category { get; private set; }
        public IReadOnlyList<Participation> RankList { get; private set; }

        private List<Participation> Classify(IReadOnlyList<Participation> participations)
        {
            var qualified = participations.Where(x => x.Category == this.Category && x.IsRanked);

            var ranked = this.Category == Category.Kids
                ? qualified.OrderBy(participation => participation.RecoverySpanInTicks)
                : qualified.OrderBy(participation => participation.FinalTime);

            return ranked.ToList();
        }
    }
}
