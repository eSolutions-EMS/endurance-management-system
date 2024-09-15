using NTS.Domain.Configuration;

namespace NTS.Domain.Core.Configuration.Regions;

public class BulgarianConfiguration : RegionalConfiguration
{
    public BulgarianConfiguration() : base(new Country("BGR", "Bulgaria")) // TODO: figure out country infra
    {
        ShouldOnlyUseAverageLoopSpeed = true;
    }
}
