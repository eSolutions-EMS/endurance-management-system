using Not.Domain.Summary;

namespace NTS.Domain.Events.Start;

public interface IStart
{
	ISummarizable Setup { get; set; }
}
