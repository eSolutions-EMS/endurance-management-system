using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MudBlazor;
using Not.Blazor.Components;
using Not.Blazor.TM.Models;
using Not.Services;
using NTS.Domain.Core.Objects;

namespace NTS.Judge.Blazor.Pages.Dashboard.Startlist;
public abstract class StartlistTable : NotComponent
{ 
    protected MudTabs _tabs = default!;
    protected List<TabModel> _userTabs = [];
    [Parameter]
    public IEnumerable<Start> Starts { get; set; } = [];

    protected override void OnParametersSet()
    {
        foreach (var start in Starts)
        {
            var tabId = Guid.NewGuid();
            var tabHeader = $"{@Localizer.Get("Gate")} {start.PhaseNumber}";
            var tab = new TabModel(tabId, tabHeader);
            if (!_userTabs.Any(t => t.Header == tab.Header))
            {
                _userTabs.Add(tab);
            }
        }
    }
}
