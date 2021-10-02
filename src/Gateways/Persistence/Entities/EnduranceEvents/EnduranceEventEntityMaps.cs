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
            profile.CreateMap<Domain.Aggregates.State.EventState, EventEntity>()
                .ForMember(d => d.Country, opt => opt.Ignore());
            profile.CreateMap<Domain.Aggregates.Import.EnduranceEvents.EnduranceEvent, EventEntity>();
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<EventEntity, EventRootModel>();
            profile.CreateMap<EventEntity, Domain.Aggregates.State.EventState>();
            profile.CreateMap<EventEntity, Domain.Aggregates.Import.EnduranceEvents.EnduranceEvent>();
            profile.CreateMap<EventEntity, ListItemModel>();
        }
    }
}
