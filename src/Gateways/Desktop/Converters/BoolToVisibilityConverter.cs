using AutoMapper;
using System.Windows;

namespace EnduranceJudge.Gateways.Desktop.Converters
{
    public class BoolToVisibilityConverter : IValueConverter<bool, Visibility>
    {
        public Visibility Convert(bool sourceMember, ResolutionContext context)
        {
            return sourceMember
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
