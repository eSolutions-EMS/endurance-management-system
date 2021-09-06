using EnduranceJudge.Core.Utilities;
using System;

namespace EnduranceJudge.Core.Models
{
    public abstract class ObjectBase : IObject
    {
        private readonly int objectUniqueCode;

        protected ObjectBase()
        {
            this.ObjectId = Guid.NewGuid();
            this.objectUniqueCode = ObjectUtilities.GetUniqueObjectCode(this);
        }

        public Guid ObjectId { get; }

        public bool Equals(IObject? other)
        {
            return ObjectUtilities.IsEqual(this, other);
        }

        public override bool Equals(object? other)
        {
            return this.Equals(other as IObject);
        }

        public override int GetHashCode()
        {
            return this.objectUniqueCode;
        }
    }
}
