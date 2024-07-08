namespace NTS.Judge.MAUI.Server.ACL.EMS;

public class EmsAthleteException : EmsDomainExceptionBase
{
    protected override string Entity { get; } = nameof(EmsAthlete);
}
