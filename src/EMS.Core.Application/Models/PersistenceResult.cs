namespace EnduranceJudge.Application.Models;

public class PersistenceResult
{
    public static PersistenceResult New => new(false);
    public static PersistenceResult Existing => new(true);

    private PersistenceResult(bool IsExistingFile)
    {
        this.IsExistingFile = IsExistingFile;
    }

    public bool IsExistingFile { get; }
}
