using System.Collections.Generic;
using Core.Models;

namespace Core.Domain.AggregateRoots.Manager.Aggregates.Startlists;

public class Startlist : ObservableCollection<StartlistEntry>
{
    public Startlist() { }

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
