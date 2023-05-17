using AutoMapper;

namespace Core.Mappings;

public interface ICustomMapConfiguration
{
    void AddFromMaps(IProfileExpression profile);
    void AddToMaps(IProfileExpression profile);
}
