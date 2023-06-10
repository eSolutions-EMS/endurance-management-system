using System;

namespace Core.Models;

public class SortedCollection<T> : ObservableCollection<T>
    where T : IEquatable<T>, IComparable<T>
{
    protected override void OnCollectionChanged()
    {
        this.Sort();
        base.OnCollectionChanged();
    }
}
