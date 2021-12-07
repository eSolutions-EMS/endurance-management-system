using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Core.Exceptions;
using System;

namespace EnduranceJudge.Domain.Core.Models
{
    public abstract class DomainObjectBase<TException> : IDomainObject, IEquatable<DomainObjectBase<TException>>
        where TException : DomainObjectException, new()
    {
        protected const string GENERATE_ID = "GenerateIdFlag";

        // Empty constructor is used by mapping for existing (in the database) entries
        protected DomainObjectBase() {}
        // Unused variable is needed mark the constructor which generates Id
        // That constructor should ONLY be used when creating NEW (no database entry) objects
        protected DomainObjectBase(string generateIdFlag)
        {
            this.Id = DomainIdProvider.Generate();
        }

        public int Id { get; protected init; } // Keep setter for mapping

        internal void Validate(Action action)
        {
            try
            {
                action();
            }
            catch (DomainException exception)
            {
                this.Throw(exception.Message);
            }
        }

        internal void Throw(string message)
            => Thrower.Throw<TException>(message);

        public override bool Equals(object other)
            => this.IsEqual(other);

        public bool Equals(IIdentifiable identifiable)
            => this.IsEqual(identifiable);

        public bool Equals(DomainObjectBase<TException> domainObject)
            => this.IsEqual(domainObject);

        public static bool operator ==(DomainObjectBase<TException> one, DomainObjectBase<TException> two)
        {
            if (ReferenceEquals(one, null))
            {
                return ReferenceEquals(two, null);
            }
            return one.Equals(two);
        }

        public static bool operator !=(DomainObjectBase<TException> one, DomainObjectBase<TException> two)
            => !(one == two);

        private bool IsEqual(object other)
        {
            if (other is not IDomainObject domainModel)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (this.Id == domainModel.Id)
            {
                return true;
            }
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return this.GetHashCode().Equals(other.GetHashCode());
        }

        public override int GetHashCode()
            => this.Id;
    }
}
