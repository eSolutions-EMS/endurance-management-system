using AutoMapper;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Enums;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Dependants.Competitions
{
    public class CompetitionViewModelMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<CompetitionViewModel, CompetitionDependantModel>()
                .MapMember(x => x.Type, y => (CompetitionType)y.Type);
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<CompetitionDependantModel, CompetitionViewModel>()
                .MapMember(x => x.Type, y => (int)y.Type);
        }
    }
}
