﻿namespace Common.Domain;

public interface IParent<T> : IParent
    where T : DomainEntity
{
    void Add(T child);
    void Remove(T child);
    void Update(T child);
}

public interface IParent
{
}