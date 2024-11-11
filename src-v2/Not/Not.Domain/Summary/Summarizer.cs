using System.Text;

namespace Not.Domain.Summary;

public class Summarizer
{
    readonly StringBuilder sb = new();

    public Summarizer(object obj)
    {
        var value = obj.ToString();
        sb.AppendLine(value);
    }

    public void Add(string label, IEnumerable<ISummarizable> summarizables)
    {
        sb.AppendLine($"= {label}");
        foreach (var summarizable in summarizables)
        {
            var summaryLine = $"= {summarizable.Summarize()}";
            sb.AppendLine(summaryLine);
        }
    }

    public override string ToString()
    {
        return sb.ToString();
    }
}
