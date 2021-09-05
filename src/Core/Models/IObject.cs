using System;

namespace EnduranceJudge.Core.Models
{
    public interface IObject : IEquatable<IObject>
    {
        Guid ObjectId { get; }
    }
}
