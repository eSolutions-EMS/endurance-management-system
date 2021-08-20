using System;

namespace EnduranceJudge.Core.Exceptions
{
    public class CoreException : Exception
    {
        public CoreException(string message)
            : base(message)
        {
        }

        public CoreException(string template, params object[] arguments)
            : base(string.Format(template, arguments))
        {
        }
    }
}
