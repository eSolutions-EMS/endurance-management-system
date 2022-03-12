using AutoMapper;
using EnduranceJudge.Core.Mappings;
using EnduranceJudge.Core.Mappings.Converters;
using EnduranceJudge.Domain.State.Laps;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Configuration.Children.Laps.Configuration
{
    public class LapViewModelMaps : ICustomMapConfiguration
    {
        public void AddFromMaps(IProfileExpression profile)
        {
            profile.CreateMap<Lap, LapViewModel>()
                .ForMember(x => x.IsFinalValue, opt => opt.ConvertUsing(new BoolToIntConverter(), y => y.IsFinal));
        }

        public void AddToMaps(IProfileExpression profile)
        {
            profile.CreateMap<LapViewModel, Lap>();
        }
    }
}
