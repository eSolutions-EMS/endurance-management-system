using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Core.Domain.Common.Extensions;
using Core.Domain.Common.Models;
using Core.Domain.Enums;
using Core.Domain.State.Laps;

namespace Core.Domain.State.Competitions;

public class Competition : DomainBase<CompetitionException>, ICompetitionState
{
    private Competition() { }

    public Competition(CompetitionType type, string name)
        : base(GENERATE_ID)
    {
        this.Type = type;
        this.Name = name;
        this.StartTime = DateTime.Now;
    }

    public Competition(ICompetitionState state)
        : base(GENERATE_ID)
    {
        this.Type = state.Type;
        this.Name = state.Name;
        this.StartTime = state.StartTime;
        FeiScheduleNumber = state.FeiScheduleNumber;
        FeiCategoryEventNumber = state.FeiCategoryEventNumber;
        EventCode = state.EventCode;
        Rule = state.Rule;
    }

    private List<Lap> laps = new();
    public CompetitionType Type { get; internal set; }
    public string Name { get; internal set; }

    /// <summary>
    /// Used to build EnduranceEvent.FEIID and Competition.FEIID. Its located at EVENT DETAIL (Search Venue->Show->ShowName->EventCode) upper right, https://data.fei.org
    /// </summary>
    public string FeiCategoryEventNumber { get; internal set; }

    /// <summary>
    /// Used to Competition.FEIID. Its located at COMPETITION DETAIL (Search Venue->Show->ShowName->EventCode->CompetitionName), upper right, https://data.fei.org
    /// </summary>
    public string FeiScheduleNumber { get; internal set; } = string.Empty;
    public string Rule { get; internal set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public string EventCode { get; internal set; }

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
