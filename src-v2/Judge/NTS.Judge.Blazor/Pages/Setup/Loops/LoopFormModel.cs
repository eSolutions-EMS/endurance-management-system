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
        Distance = 20;
        Recovery = 8;
        Rest = 40;
        IsFinal = false;
    }
    public LoopFormModel(Loop loop)
    {
        Id = loop.Id;
        Distance = loop.Distance;
        Recovery = loop.Recovery;
        Rest = loop.Rest;
        IsFinal = loop.IsFinal;
    }

    public int Id { get; set; }
    public double Distance { get; set; }
    public int Recovery { get; set; }
    public int Rest { get; set; }
    public bool IsFinal { get; set; }
}
