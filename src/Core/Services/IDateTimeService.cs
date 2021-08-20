using System;
using EnduranceJudge.Core.ConventionalServices;

namespace EnduranceJudge.Core.Services
{
    public interface IDateTimeService : IService
    {
        DateTime Now { get; }
    }
}
