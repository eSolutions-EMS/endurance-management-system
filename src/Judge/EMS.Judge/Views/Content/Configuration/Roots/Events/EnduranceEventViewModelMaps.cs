using System.Collections.ObjectModel;
using AutoMapper;
using Core.Domain.State.Countries;
using Core.Domain.State.EnduranceEvents;
using Core.Mappings;
using EMS.Judge.Application.Aggregates.Configurations;
using EMS.Judge.Application.Common.Models;
using EMS.Judge.Views.Content.Configuration.Children.Personnel;

namespace EMS.Judge.Views.Content.Configuration.Roots.Events;

public class EnduranceEventViewModelMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile
            .CreateMap<EnduranceEvent, EnduranceEventViewModel>()
            .AfterMap(
                (s, d) =>
                {
                    var personnel = PersonnelAggregator
                        .Aggregate(s)
                        .MapEnumerable<PersonnelViewModel>();
                    d.Personnel.AddRange(personnel);
                }
            );
        profile.CreateMap<Country, ListItemModel>();
    }

    public void AddToMaps(IProfileExpression profile) { }
}
