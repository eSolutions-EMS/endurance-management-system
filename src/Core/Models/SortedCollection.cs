namespace Core.Models;

public class SortedCollection<T> : ObservableCollection<T>
{
    protected override void OnCollectionChanged()
    {
        this.Sort();
        base.OnCollectionChanged();
    }
}
