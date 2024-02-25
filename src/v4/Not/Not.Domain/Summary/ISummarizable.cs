namespace Common.Domain.Summary;

public interface ISummarizable
{
	string Summarize() => this.ToString()!;
}