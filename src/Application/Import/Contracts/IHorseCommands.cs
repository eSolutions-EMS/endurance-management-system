﻿using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Import.Contracts
{
    public interface IHorseCommands : ICommands<Horse>
    {
        Task Create(IEnumerable<Horse> domainModels, CancellationToken token);
    }
}
