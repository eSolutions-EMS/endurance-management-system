using Core.Domain.Common.Models;
using Core.Domain.Enums;
using Core.Domain.State.Laps;
using Core.Domain.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace Core.Domain.State.Competitions;

public class Competition : DomainBase<CompetitionException>, ICompetitionState
{
    private Competition() {}
    public Competition(CompetitionType type, string name) : base(GENERATE_ID)
    {
        this.Type = type;
        this.Name = name;
        this.StartTime = DateTime.Now;
    }
    public Competition(ICompetitionState state) : base(GENERATE_ID)
    {
        this.Type = state.Type;
        this.Name = state.Name;
        this.StartTime = state.StartTime;
        FeiId = state.FeiId;
        FeiScheduleNumber = state.FeiScheduleNumber;
        Rule = state.Rule;
    }

    private List<Lap> laps = new();
    public CompetitionType Type { get; internal set; }
    public string Name { get; internal set; }
    public string FeiId { get; internal set; } = string.Empty;
    public string FeiScheduleNumber { get; internal set; } = string.Empty;
    public string Rule { get; internal set; } = string.Empty;
    public DateTime StartTime { get; set; }

    public void Save(Lap lap)
    {
        this.laps.AddOrUpdate(lap);
    }

    public IReadOnlyList<Lap> Laps
    {
        get => this.laps.OrderBy(x => x.OrderBy).ToList().AsReadOnly();
        private set { this.laps = value.ToList(); }
    }
}
