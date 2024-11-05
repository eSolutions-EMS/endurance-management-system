using Not.Application.Adapters.Behinds;
using Not.Application.Ports.CRUD;
using Not.Exceptions;
using Not.TimerUtility;
using NTS.Domain;
using NTS.Domain.Core.Entities;
using NTS.Domain.Core.Objects;
using NTS.Judge.Blazor.Ports;
using System.Diagnostics;
using System.Timers;

namespace NTS.Judge.Adapters.Behinds;
public class StartlistBehind : ObservableBehind, IStartlistBehind
{
    private const double TimerTickInMilliseconds = 60000;
    private const int ExpirationTimeOfPreviousStartsInMinutes = 15;
    private readonly IRepository<Participation> _participationRepository;

    public StartlistBehind(IRepository<Participation> participations)
    {
        _participationRepository = participations;
    }
    public StartList? StartList { get; private set; }
    public List<Start> UpcomingStarts { get; private set; } = new List<Start>();
    public List<Start> StartHistory { get; private set; } = new List<Start>();
    public TimerUtility _timer { get; set; }

    protected override async Task<bool> PerformInitialization(params IEnumerable<object> arguments)
    {
        var participations = await _participationRepository.ReadAll();
        var startlist = new StartList(participations);
        GuardHelper.ThrowIfDefault(startlist);
        UpcomingStarts = startlist.UpcomingStarts;
        StartHistory = startlist.PreviousStarts;
        StartList = startlist;
        _timer = new TimerUtility(TimerTickInMilliseconds, upcomingStartExpire, () => true);
        return UpcomingStarts.Any() && StartHistory.Any();
    }

    private void upcomingStartExpire(object? sender, ElapsedEventArgs e)
    {
        foreach (var start in UpcomingStarts.ToList())
        {
            var now = new Timestamp(DateTime.Now);
            if (now - start.StartAt > TimeSpan.FromMinutes(ExpirationTimeOfPreviousStartsInMinutes))
            {
                UpcomingStarts.Remove(start);
                StartHistory.Add(start);
                EmitChange();
            }
        }
    }
}