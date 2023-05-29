using Core.Events;
using System;
using System.Collections.Generic;

namespace Core.Models;
public class ObservableCollection<T> : List<T>
{
	public new void Add(T item)
	{
		base.Add(item);
		this.OnCollectionChanged();
	}

	public new void Clear()
	{
		base.Clear();
		this.OnCollectionChanged();
	}

	public new void Insert(int index, T item)
	{
		base.Insert(index, item);
		this.OnCollectionChanged();
	}

	public new void RemoveAt(int index)
	{
		base.RemoveAt(index);
		this.OnCollectionChanged();
	}

	public new void Remove(T item)
	{
		base.Remove(item);
		this.OnCollectionChanged();
	}

	public void Remove(IEnumerable<T> items)
	{
		foreach (var item in items)
		{
			base.Remove(item);
		}
		this.OnCollectionChanged();
	}

	public new void AddRange(IEnumerable<T> collection)
	{
		base.AddRange(collection);
		this.OnCollectionChanged();
	}

	public new void RemoveAll(Predicate<T> predicate) 
	{ 
		base.RemoveAll(predicate); 
		this.OnCollectionChanged();
	}


	protected virtual void OnCollectionChanged()
	{
		AppState.RaiseChanged(null, this);
	}
}
