using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using System.Threading.Tasks;

namespace Core.Application.Api
{
	public interface IStartlistClient
	{
		Task AddEntry(StartModel entry);
	}
}
