using Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;
using System.Threading.Tasks;

namespace Core.Application.Rpc.Procedures;

public interface IStartlistProcedures
{
    Task AddEntry(StartModel entry);
}
