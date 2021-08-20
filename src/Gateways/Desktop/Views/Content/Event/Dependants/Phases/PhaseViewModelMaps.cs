using AutoMapper;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Mappings.Converters;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Phases
{
    public class PhaseViewModelMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<PhaseDependantModel, PhaseViewModel>()
                .ForMember(x => x.IsFinalValue, opt => opt.ConvertUsing(new BoolToIntConverter()));
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<PhaseViewModel, PhaseDependantModel>();
        }
    }
}
