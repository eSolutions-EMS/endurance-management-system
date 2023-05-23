using Core.Events;
using System;
using System.Collections.Generic;

namespace Core.Models;
public class ObservableCollection<T> : List<T>
{
	public new void Add(T item)
	{
		base.Add(item);
		this.RaiseChanged();
	}

	public new void Clear()
	{
		base.Clear();
		this.RaiseChanged();
	}

	public new void Insert(int index, T item)
	{
		base.Insert(index, item);
		this.RaiseChanged();
	}

	public new void RemoveAt(int index)
	{
		base.RemoveAt(index);
		this.RaiseChanged();
	}

	public new void Remove(T item)
	{
		base.Remove(item);
		this.RaiseChanged();
	}

	public new void AddRange(IEnumerable<T> collection)
	{
		base.AddRange(collection);
		this.RaiseChanged();
	}

	public new void RemoveAll(Predicate<T> predicate) 
	{ 
		base.RemoveAll(predicate); 
		this.RaiseChanged();
	}


	private void RaiseChanged()
	{
		AppState.RaiseChanged(null, this);
	}
}
