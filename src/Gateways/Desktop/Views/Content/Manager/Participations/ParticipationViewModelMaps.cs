using AutoMapper;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using System.Linq;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Manager.Participations
{
    public class ParticipationViewModelMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<Participation, ParticipationViewModel>()
                .ForMember(
                    d => d.ParticipationsInPhases,
                    opt => opt.MapFrom(s => s.ParticipationsInCompetitions
                        .FirstOrDefault()
                        .ParticipationsInPhases));
        }

        public void AddToMaps(IProfileExpression profile)
        {
        }
    }
}
