using AutoMapper;
using EMS.Judge.Views.Content.Configuration.Children.Personnel;
using EMS.Core.Application.Aggregates.Configurations;
using EMS.Core.Application.Core.Models;
using EMS.Core.Mappings;
using EMS.Core.Domain.State.Countries;
using EMS.Core.Domain.State.EnduranceEvents;
using System.Collections.ObjectModel;

namespace EMS.Judge.Views.Content.Configuration.Roots.Events;

public class EnduranceEventViewModelMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<EnduranceEvent, EnduranceEventViewModel>()
            .AfterMap((s, d) =>
            {
                var personnel = PersonnelAggregator
                    .Aggregate(s)
                    .MapEnumerable<PersonnelViewModel>();
                d.Personnel.AddRange(personnel);
            });
        profile.CreateMap<Country, ListItemModel>();

    }

    public void AddToMaps(IProfileExpression profile)
    {
    }
}
