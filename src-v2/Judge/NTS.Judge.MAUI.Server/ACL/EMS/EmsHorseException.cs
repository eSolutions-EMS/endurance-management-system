namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsHorseException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsHorse);
}
