using AutoMapper;
using EMS.Judge.Application.Core.Models;
using Core.Extensions;
using Core.Mappings;
using Core.Domain.State.Athletes;

namespace EMS.Judge.Views.Content.Configuration.Roots.Athletes.Configuration;

public class AthleteMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<Athlete, AthleteViewModel>()
            .MapMember(x => x.CategoryId, y => (int)y.Category);
        profile.CreateMap<Athlete, ListItemModel>();
    }

    public void AddToMaps(IProfileExpression profile)
    {
    }
}
