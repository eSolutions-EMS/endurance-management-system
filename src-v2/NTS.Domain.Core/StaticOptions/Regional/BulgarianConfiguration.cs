using NTS.Domain.Core.StaticOptions.Regional.Base;

namespace NTS.Domain.Core.StaticOptions.Regional;

public class BulgarianConfiguration : RegionalConfiguration
{
    public BulgarianConfiguration()
        : base("BGR") // TODO: figure out country infra
    {
        ShouldOnlyUseAverageLoopSpeed = true;
    }
}
