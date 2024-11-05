using AutoMapper;
using Core.Domain.State.Laps;
using Core.Mappings;
using Core.Mappings.Converters;

namespace EMS.Judge.Views.Content.Configuration.Children.Laps.Configuration;

public class LapViewModelMaps : ICustomMapConfiguration
{
    public void AddFromMaps(IProfileExpression profile)
    {
        profile.CreateMap<Lap, LapViewModel>();
    }

    public void AddToMaps(IProfileExpression profile)
    {
        profile.CreateMap<LapViewModel, Lap>();
    }
}
