using System;

namespace EnduranceJudge.Application.Core.Exceptions
{
    public class AppException : Exception
    {
        public AppException(string message) : base(message)
        {
        }
    }
}
