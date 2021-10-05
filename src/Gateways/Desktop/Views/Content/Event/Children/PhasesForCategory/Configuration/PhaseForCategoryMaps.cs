using AutoMapper;
using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.PhasesForCategory;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.PhasesForCategory.Configuration
{
    public class PhaseForCategoryMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<PhaseForCategory, PhaseForCategoryViewModel>()
                .MapMember(x => x.CategoryId, y => (int)y.Category);
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<PhaseForCategoryViewModel, PhaseForCategory>();
        }
    }
}
