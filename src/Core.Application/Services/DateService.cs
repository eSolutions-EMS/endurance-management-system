using Core.ConventionalServices;
using System;

namespace Core.Application.Services;

public class DateService : IDateService
{
    public string FormatTime(DateTime time, bool showMs = false)
        => showMs
            ? time.ToString("HH:mm:ss.fff")
            : time.ToString("HH:mm:ss");

	public DateTime Now()
        => DateTime.Now;  
}

public interface IDateService : ITransientService
{
    DateTime Now();
    string FormatTime(DateTime time, bool showMs = false);

}
