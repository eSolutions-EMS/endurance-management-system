using Core.Models;
using System.Collections.Generic;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class Startlist : ObservableCollection<StartlistEntry>
{
    public Startlist()
    {
    }

    public Startlist(IEnumerable<StartlistEntry> entries)
    {
        this.AddRange(entries);
    }

    protected override void OnCollectionChanged()
    {
        this.Sort();
        base.OnCollectionChanged();
    }
}
