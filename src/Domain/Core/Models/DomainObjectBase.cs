using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Core.Exceptions;
using System;

namespace EnduranceJudge.Domain.Core.Models
{
    public abstract class DomainObjectBase<TException> : IDomainObject
        where TException : DomainObjectException, new()
    {
        private readonly int id;

        protected DomainObjectBase() {}

        protected DomainObjectBase(int id)
        {
            this.Id = id;
        }

        public int Id
        {
            get => this.id;
            private init
            {
                this.id = value == default
                    ? DomainIdProvider.Generate()
                    : value;
            }
        }

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
            => base.GetHashCode() + this.Id;
    }
}
