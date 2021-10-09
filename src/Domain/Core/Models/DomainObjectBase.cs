using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Core.Exceptions;
using System;

namespace EnduranceJudge.Domain.Core.Models
{
    public abstract class DomainObjectBase<TException> : IDomainObject
        where TException : DomainObjectException, new()
    {
        protected DomainObjectBase() {}
        protected DomainObjectBase(bool targetThisConstructor)
        {
            this.Id = DomainIdProvider.NewId();
        }

        public int Id { get; init; }

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
        internal void Validate<TCustomException>(Action action)
            where TCustomException : DomainObjectException, new()
        {
            try
            {
                action();
            }
            catch (DomainException exception)
            {
                this.Throw<TCustomException>(exception.Message);
            }
        }

        internal void Throw(string message)
            => Thrower.Throw<TException>(message);
        internal void Throw<TCustomException>(string message)
            where TCustomException : DomainObjectException, new()
            => Thrower.Throw<TCustomException>(message);

        public override bool Equals(object other)
        {
            if (other is not IDomainObject domainModel)
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            if (this.GetType() != other.GetType())
            {
                return false;
            }

            return this.GetHashCode().Equals(other.GetHashCode());
        }

        public bool Equals(IIdentifiable identifiable)
        {
            if (identifiable is not IDomainObject domainObject)
            {
                return false;
            }
            if (this.Id != default &&  identifiable.Id != default)
            {
                return this.Id == identifiable.Id;
            }

            return this.Equals(domainObject);
        }

        public override int GetHashCode()
            => base.GetHashCode() + this.Id;
    }
}
