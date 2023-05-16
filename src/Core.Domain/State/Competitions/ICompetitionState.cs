using EMS.Core.Domain.Enums;
using EMS.Core.Models;
using System;

namespace EMS.Core.Domain.State.Competitions;

public interface ICompetitionState : IIdentifiable
{
    CompetitionType Type { get; }
    string Name { get; }
    DateTime StartTime { get; }
}
