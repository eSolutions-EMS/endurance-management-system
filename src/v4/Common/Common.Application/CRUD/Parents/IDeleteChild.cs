﻿using Common.Conventions;
using Common.Domain;

namespace Common.Application.CRUD.Parents;

public interface IDeleteChild<T> : ITransientService
    where T : DomainEntity
{
    Task Delete(int parentId, T child);
}