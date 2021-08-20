using AutoMapper;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Core.Extensions;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;

namespace EnduranceJudge.Gateways.Persistence.Entities.Athletes
{
    public class AthleteEntityMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<Athlete, AthleteEntity>();
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<AthleteEntity, AthleteRootModel>();
            profile.CreateMap<AthleteEntity, ListItemModel>()
                .MapMember(x => x.Name, y => $"{y.FirstName} {y.LastName}");
        }
    }
}
