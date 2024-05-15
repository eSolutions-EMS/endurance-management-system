using NTS.Domain.Setup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Blazor.Pages.Setup.Phases;
public class PhaseFormModel
{
    public PhaseFormModel()
    {
        // mock data for testing
        Loop = 20;
        Recovery = 15;
        Rest = 40;
    }
    public PhaseFormModel(Phase phase)
    {
        Id = phase.Id;
        Loop = phase.Loop.Distance;
        Recovery = phase.Recovery;
        Rest = phase.Rest;
    }

    public int Id { get; set; }
    public double Loop { get; set; }
    public int Recovery { get; set; }
    public int Rest { get; set; }
    public bool IsFinal { get; set; }
}
