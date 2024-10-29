using Not.Application.Ports.CRUD;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Judge.Blazor.Ports;

namespace NTS.Judge.Adapters.Behinds;
public class StartlistBehind : IStartlistBehind
{
    private readonly IRepository<Participation> _participationRepository;

    public StartlistBehind(IRepository<Participation> participations)
    {
        _participationRepository = participations;
    }

    public IEnumerable<Start> UpcomingStarts { get; private set; } = default!;
    public IEnumerable<Start> StartHistory { get; private set; } = default!;

    public async Task Initialize()
    {
        var participations = await _participationRepository.ReadAll();
        var startlist = new StartList(participations);
        UpcomingStarts = startlist.UpcomingStarts;
        StartHistory = startlist.PreviousStarts;
    }
}
