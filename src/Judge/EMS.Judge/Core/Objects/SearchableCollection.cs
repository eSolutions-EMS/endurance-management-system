using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EMS.Judge.Core.Objects;

public class SearchableCollection<TItem> : ObservableCollection<TItem>
{
    private readonly Func<TItem, string, bool> filter;
    private List<TItem> allItems = new();

    public SearchableCollection(Func<TItem, string, bool> filter) : this(Enumerable.Empty<TItem>().ToList(), filter)
    {
    }

    public SearchableCollection(List<TItem> items, Func<TItem, string, bool> filter) : base(items)
    {
        this.filter = filter;
    }

    public void Search(string value)
    {
        this.allItems = this.ToList();
        var notMatches = this
            .Where(x => !this.filter(x, value))
            .ToList();
        foreach (var notMatch in notMatches)
        {
            this.Remove(notMatch);
        }
    }

    public void ClearSearch()
    {
        this.ClearItems();
        this.AddRange(this.allItems);
    }
}
