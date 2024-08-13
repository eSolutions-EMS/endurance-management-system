using System.Collections;
using System.Collections.ObjectModel;

namespace Not.Concurrency;

public class ConcurrentList<T> : IEnumerable<T>
{
    private readonly List<T> _items = new();
    private readonly object _lock = new ();

    public ConcurrentList()
    {
        _items = new();
    }
    public ConcurrentList(IEnumerable<T> enumerable)
    {
        _items = enumerable.ToList();
    }

    public void Add(T item)
    {
        lock(_lock)
        {
            _items.Add(item);
        }
    }

    public void AddRange(IEnumerable<T> collection)
    {
        lock(_lock)
        {
            _items.AddRange(collection);
        }
    }

    public void Remove(T item)
    {
        lock(_lock)
        {
            _items.Remove(item);
        }
    }

    public void RemoveRange(IEnumerable<T> collection)
    {
        lock(_lock)
        {
            foreach (var item in collection)
            {
                _items.Remove(item);
            }
        }
    }

    public List<T> PopAll()
    {
        lock (_lock)
        {
            var result = _items.ToList();
            _items.Clear();
            return result;
        }
    }

    public void Clear()
    {
        lock (_lock)
        {
            _items.Clear();
        }
    }

    public ReadOnlyCollection<T> AsReadOnly()
    {
        return _items.AsReadOnly();
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
