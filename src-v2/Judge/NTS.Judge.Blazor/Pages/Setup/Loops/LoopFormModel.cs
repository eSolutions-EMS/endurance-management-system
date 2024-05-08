using NTS.Domain.Setup.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Blazor.Pages.Setup.Loops;
public class LoopFormModel
{
    public LoopFormModel()
    {
        // mock data for testing
        Phase = 20;
        Recovery = 15;
        Rest = 40;
    }
    public LoopFormModel(Loop loop)
    {
        Id = loop.Id;
        Phase = loop.Phase.Distance;
        Recovery = loop.Recovery;
        Rest = loop.Rest;
    }

    public int Id { get; set; }
    public double Phase { get; set; }
    public int Recovery { get; set; }
    public int Rest { get; set; }
    public bool IsFinal { get; set; }
}
