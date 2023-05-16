using AutoMapper;
using EMS.Core.Mappings;
using EMS.Core.Mappings.Converters;
using EMS.Core.Domain.State.Laps;

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
