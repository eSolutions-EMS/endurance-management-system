using NTS.Domain.Core.StaticOptions.Regional.Base;

namespace NTS.Domain.Core.StaticOptions.Regional;

public class BulgariaOption : RegionalOption
{
    public BulgariaOption()
        : base("BGR") // TODO: figure out country infra
    {
        ShouldOnlyUseAverageLoopSpeed = true;
    }
}
