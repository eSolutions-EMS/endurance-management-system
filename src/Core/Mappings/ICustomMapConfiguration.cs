using AutoMapper;

namespace EnduranceJudge.Core.Mappings
{
    public interface ICustomMapConfiguration
    {
        void AddFromMaps(IProfileExpression profile);
        void AddToMaps(IProfileExpression profile);
    }
}
