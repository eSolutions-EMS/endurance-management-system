using EnduranceJudge.Domain.Enums;
using System;
using System.Collections.Generic;

namespace EnduranceJudge.Domain.Aggregates.Manager.DTOs
{
    public class CompetitionDto
    {
        public DateTime StartTime { get; set; }

        public CompetitionType Type { get; set; }

        public IReadOnlyList<PhaseDto> Phases { get; set; }
    }
}
