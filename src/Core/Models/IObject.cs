using System;

namespace EnduranceJudge.Core.Models
{
    public interface IObject : IEquatable<IObject>
    {
        Guid ObjectId { get; }

        bool ObjectEquals(IObject other);
    }
}
