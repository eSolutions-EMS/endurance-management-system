using EnduranceJudge.Core.Exceptions;
using EnduranceJudge.Core.Models;
using EnduranceJudge.Domain.Core.Exceptions;
using System;

namespace EnduranceJudge.Domain.Core.Models
{
    public abstract class DomainBase<TException> : ObjectBase, IDomainModel
        where TException : DomainException, new()
    {
        protected DomainBase()
        {
        }

        protected DomainBase(int id)
        {
            this.Id = id;
        }

        public int Id { get; private set; }

        internal void Validate(Action action)
        {
            try
            {
                action();
            }
            catch (CoreException exception)
            {
                this.Throw(exception.Message);
            }
        }

        internal void Throw(string message)
            => Thrower.Throw<TException>(message);

        public override bool Equals(IObject other)
        {
            return this.IsEqual(other);
        }

        public override bool Equals(object other)
        {
            return this.IsEqual(other);
        }

        public bool Equals(IIdentifiable identifiable)
        {
            if (this.Id != default &&  identifiable.Id != default)
            {
                return this.Id == identifiable.Id;
            }

            return base.Equals(identifiable);
        }

        public override int GetHashCode()
            => base.GetHashCode() + this.Id;

        private bool IsEqual(object other)
        {
            if (other is not IDomainModel domainModel)
            {
                return false;
            }

            if (this.GetType() != domainModel.GetType())
            {
                return false;
            }

            return this.Equals(domainModel);
        }
    }
}
