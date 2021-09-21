using AutoMapper;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Application.Events.Queries.GetEvent;
using EnduranceJudge.Core.Mappings;

namespace EnduranceJudge.Gateways.Persistence.Entities.EnduranceEvents
{
    public class EnduranceEventEntityMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<Domain.Aggregates.Event.EnduranceEvents.EnduranceEvent, EnduranceEventEntity>()
                .ForMember(d => d.Country, opt => opt.Ignore());
            profile.CreateMap<Domain.Aggregates.Import.EnduranceEvents.EnduranceEvent, EnduranceEventEntity>();
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<EnduranceEventEntity, EnduranceEventRootModel>();
            profile.CreateMap<EnduranceEventEntity, Domain.Aggregates.Event.EnduranceEvents.EnduranceEvent>();
            profile.CreateMap<EnduranceEventEntity, Domain.Aggregates.Import.EnduranceEvents.EnduranceEvent>();
            profile.CreateMap<EnduranceEventEntity, ListItemModel>();
        }
    }
}
