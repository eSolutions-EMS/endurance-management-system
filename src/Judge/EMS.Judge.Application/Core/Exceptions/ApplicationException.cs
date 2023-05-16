using System;

namespace EMS.Judge.Application.Core.Exceptions;

public class AppException : Exception
{
    public AppException(string message) : base(message)
    {
    }
}