using Newtonsoft.Json;
using System;

namespace EnduranceJudge.Core.Models
{
    public abstract class ObjectBase : IObject
    {
        protected ObjectBase()
        {
            this.ObjectId = Guid.NewGuid();
        }

        [JsonIgnore]
        public Guid ObjectId { get; }


        // TODO: Should be able to completely replace this
        // With ReferenceEquals and greatly simplify the
        // Equals inheritance chain
        public bool ObjectEquals(IObject other)
            => this.ObjectId == other.ObjectId;

        public virtual bool Equals(IObject other)
        {
            return this.IsEqual(other);
        }

        public override bool Equals(object? other)
        {
            if (other is not IObject obj)
            {
                return false;
            }

            return this.IsEqual(obj);
        }

        public override int GetHashCode()
            => (this.GetType().ToString() + this.ObjectId).GetHashCode();

        private bool IsEqual(IObject other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (other.GetType() != this.GetType())
            {
                return false;
            }

            return this.ObjectId.Equals(other.ObjectId);
        }
    }
}
