using AutoMapper;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Manager.Participations;
using EnduranceJudge.Gateways.Desktop.Converters;
using System.Linq;
using System.Windows;

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
                        .ParticipationsInPhases))
                .ForMember(
                    d => d.StartVisibility,
                    opt => opt.ConvertUsing(new BoolToVisibilityConverter(), s => s.CanStart))
                .ForMember(
                    d => d.ArriveVisibility,
                    opt => opt.ConvertUsing(new BoolToVisibilityConverter(), s => s.CanArrive))
                .ForMember(
                    d => d.InspectVisibility,
                    opt => opt.ConvertUsing(new BoolToVisibilityConverter(), s => s.CanInspect))
                .ForMember(
                    d => d.ReInspectVisibility,
                    opt => opt.ConvertUsing(new BoolToVisibilityConverter(), s => s.CanReInspect))
                .ForMember(
                    d => d.CompleteVisibility,
                    opt => opt.ConvertUsing(new BoolToVisibilityConverter(), s => s.CanComplete));
        }

        public void AddToMaps(IProfileExpression profile)
        {
        }
    }
}
