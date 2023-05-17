using AutoMapper;
using Core.Mappings;
using Core.Mappings.Converters;
using Core.Domain.State.Horses;

namespace EMS.Judge.Views.Content.Configuration.Roots.Horses.Configuration;

public class HorseViewModelMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<Horse, HorseViewModel>();
        //     .ForMember(
        //         x => x.IsStallion,
        //         opt => opt.ConvertUsing(new BoolToIntConverter(), x => x.IsStallion));
    }

    public void AddToMaps(IProfileExpression profile)
    {
        profile.CreateMap<HorseViewModel, Horse>();
    }
}
