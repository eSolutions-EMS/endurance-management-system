namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsCompetitionException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsCompetition);
}
