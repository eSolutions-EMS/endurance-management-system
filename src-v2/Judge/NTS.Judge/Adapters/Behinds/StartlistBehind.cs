using Not.Application.Adapters.Behinds;
using Not.Application.Ports.CRUD;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;

public class StartlistBehind : ObservableBehind, IStartlistUpcoming, IStartlistHistory
{
    readonly IRepository<Participation> _participationRepository;
    StartList? _startlist;

    public StartlistBehind(IRepository<Participation> participationRepository)
    {
        _participationRepository = participationRepository;
    }

    public IReadOnlyList<Start> Upcoming => _startlist?.Upcoming ?? [];
    public IReadOnlyList<Start> History => _startlist?.History ?? [];

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        var participations = await _participationRepository.ReadAll();
        _startlist = new StartList(participations, EmitChange);
        ;
        return _startlist.Any();
    }
}
