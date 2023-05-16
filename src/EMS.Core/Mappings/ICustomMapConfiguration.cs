using AutoMapper;

namespace EMS.Core.Mappings;

public interface ICustomMapConfiguration
{
    void AddFromMaps(IProfileExpression profile);
    void AddToMaps(IProfileExpression profile);
}
