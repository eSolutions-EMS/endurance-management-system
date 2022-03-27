namespace EnduranceJudge.Application.Contracts;

public interface IPersistence
{
    void Snapshot();
    string LogError(string error);
}
