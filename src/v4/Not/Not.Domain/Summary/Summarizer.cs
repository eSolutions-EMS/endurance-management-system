using System.Text;

namespace Not.Domain.Summary;

public class Summarizer
{
    private readonly StringBuilder sb = new();

    public Summarizer(object obj)
    {
        this.sb.AppendLine(obj.ToString());
    }

    public void Add(string label, IEnumerable<ISummarizable> summarizables)
    {
        this.sb.AppendLine($"= {label}");
        foreach (var summarizable in summarizables)
        {
            this.sb.AppendLine($"= {summarizable.Summarize()}");
        }
    }

	public override string ToString()
	{
		return this.sb.ToString();
	}
}
