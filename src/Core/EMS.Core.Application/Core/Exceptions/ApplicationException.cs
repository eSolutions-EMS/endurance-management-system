using System;

namespace EMS.Core.Application.Core.Exceptions;

public class AppException : Exception
{
    public AppException(string message) : base(message)
    {
    }
}