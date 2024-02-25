namespace Not.Domain.Summary;

public interface ISummarizable
{
	string Summarize() => this.ToString()!;
}