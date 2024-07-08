namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsLapException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsLap);
}
