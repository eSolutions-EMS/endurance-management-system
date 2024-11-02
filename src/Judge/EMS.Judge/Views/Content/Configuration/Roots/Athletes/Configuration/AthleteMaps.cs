using AutoMapper;
using Core.Domain.State.Athletes;
using Core.Extensions;
using Core.Mappings;
using EMS.Judge.Application.Common.Models;

namespace EMS.Judge.Views.Content.Configuration.Roots.Athletes.Configuration;

public class AthleteMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile
            .CreateMap<Athlete, AthleteViewModel>()
            .MapMember(x => x.CategoryId, y => (int)y.Category);
        profile.CreateMap<Athlete, ListItemModel>();
    }

    public void AddToMaps(IProfileExpression profile) { }
}
