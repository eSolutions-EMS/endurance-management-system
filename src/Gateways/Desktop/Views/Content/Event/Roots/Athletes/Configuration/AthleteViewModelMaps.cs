using AutoMapper;
using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.Athletes;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.Athletes.Configuration
{
    public class AthleteViewModelMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<Athlete, AthleteViewModel>()
                .MapMember(x => x.CategoryId, y => (int)y.Category);
        }

        public void AddToMaps(IProfileExpression profile)
        {
        }
    }
}
