namespace EMS.Domain.Events.Start;

public interface IStartEvent
{
	ISummarizable Setup { get; set; }
}
