using AutoMapper;
using Core.Mappings;
using Core.Mappings.Converters;
using Core.Domain.State.Laps;

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
