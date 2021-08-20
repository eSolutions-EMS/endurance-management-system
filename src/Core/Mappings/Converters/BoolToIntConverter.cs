using AutoMapper;

namespace EnduranceJudge.Core.Mappings.Converters
{
    public class BoolToIntConverter : IValueConverter<bool, int>
    {
        public int Convert(bool sourceMember, ResolutionContext context)
        {
            var result = sourceMember ? 1 : 0;
            return result;
        }
    }
}
