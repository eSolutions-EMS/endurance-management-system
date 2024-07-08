namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsParticipationException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsParticipation);
}
