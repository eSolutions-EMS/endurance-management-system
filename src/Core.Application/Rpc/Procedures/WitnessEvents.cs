using Core.Domain.AggregateRoots.Manager;

namespace Core.Application.Rpc.Procedures;
public interface IWitnessEventsHubProcedures
{
	void Add(WitnessEvent witnessEvent);
}
