using System;

namespace EnduranceJudge.Domain.Core.Exceptions
{
    public abstract class DomainException : Exception
    {
        public string DomainMessage { private get; set; }

        public override string Message
            => string.Format($"{this.Entity} {this.DomainMessage}");

        protected abstract string Entity { get; }
    }
}
