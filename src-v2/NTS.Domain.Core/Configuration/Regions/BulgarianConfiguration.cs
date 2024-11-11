namespace NTS.Domain.Core.Configuration.Regions;

public class BulgarianConfiguration : RegionalConfiguration
{
    public BulgarianConfiguration()
        : base("BGR") // TODO: figure out country infra
    {
        ShouldOnlyUseAverageLoopSpeed = true;
    }
}
