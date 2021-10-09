using AutoMapper;
using EnduranceJudge.Application.Aggregates.Configurations;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Domain.State.EnduranceEvents;
using EnduranceJudge.Gateways.Desktop.Views.Content.Event.Children.Personnel;
using System.Collections.ObjectModel;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Event.Roots.EnduranceEvents
{
    public class EnduranceEventViewModelMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<EnduranceEvent, EnduranceEventViewModel>()
                .AfterMap((s, d) =>
                {
                    var personnel = PersonnelAggregator
                        .Aggregate(s)
                        .MapEnumerable<PersonnelViewModel>();
                    d.Personnel.AddRange(personnel);
                });
        }
    }
}
