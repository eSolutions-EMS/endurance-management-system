namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsParticipantException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsParticipant);
}
