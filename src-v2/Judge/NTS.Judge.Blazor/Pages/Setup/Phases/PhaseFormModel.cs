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
        Recovery = 15;
        Rest = 40;
    }
    public PhaseFormModel(Phase phase)
    {
        Id = phase.Id;
        Loop = phase.Loop;
        Recovery = (int)phase.Recovery!;
        Rest = (int)phase.Rest!;
    }

    public int Id { get; set; }
    public Loop Loop { get; set; }
    public int Recovery { get; set; }
    public int Rest { get; set; }
}
