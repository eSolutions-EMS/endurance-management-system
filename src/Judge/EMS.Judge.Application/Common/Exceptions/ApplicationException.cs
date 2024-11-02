using System;

namespace EMS.Judge.Application.Common.Exceptions;

public class AppException : Exception
{
    public AppException(string message)
        : base(message) { }
}
