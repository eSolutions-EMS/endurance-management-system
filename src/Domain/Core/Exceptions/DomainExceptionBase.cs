using System;

namespace EnduranceJudge.Domain.Core.Exceptions
{
    public abstract class DomainExceptionBase : Exception
    {
        protected abstract string Entity { get; }
        protected string InitMessage { get; init; }
        public override string Message => this.Prefix(this.InitMessage ?? this.Message);

        public static T Create<T>(string message) where T : DomainExceptionBase, new()
        {
            var exception = new T
            {
                InitMessage = message,
            };
            return exception;
        }
        public static T Create<T>(string message, params object[] arguments) where T : DomainExceptionBase, new()
        {
            var exception = new T
            {
                InitMessage = string.Format(message, arguments),
            };
            return exception;
        }
        public static DomainException Create(string entity, string message)
        {
            var exception = new DomainException(entity, message);
            return exception;
        }
        public static DomainException Create(string entity, string message, params object[] arguments)
        {
            var exception = new DomainException(entity, message, arguments);
            return exception;
        }

        private string Prefix(string message)
        {
            return $"{this.Entity} {message}";
        }
    }
}
