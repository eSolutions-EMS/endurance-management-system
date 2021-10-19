using System;

namespace EnduranceJudge.Core.Models
{
    public interface IIdentifiable : IEquatable<IIdentifiable>
    {
        int Id { get; }
    }
}
