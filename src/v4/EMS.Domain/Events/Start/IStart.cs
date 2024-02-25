using Not.Domain.Summary;

namespace EMS.Domain.Events.Start;

public interface IStart
{
	ISummarizable Setup { get; set; }
}
