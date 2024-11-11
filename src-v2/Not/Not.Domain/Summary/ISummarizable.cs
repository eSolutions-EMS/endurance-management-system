namespace Not.Domain.Summary;

public interface ISummarizable
{
    string Summarize()
    {
        return ToString()!;
    }
}
