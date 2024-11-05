using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NTS.Judge.Blazor.Pages.Dashboard.Startlist;
public class TabView
{
    public string Label { get; set; }
    public Guid Id { get; set; }
    public double Key { get; set; }
    public bool ShowCloseIcon { get; set; } = true;
}