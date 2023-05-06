using EnduranceJudge.Core.ConventionalServices;
using System;

namespace EnduranceJudge.Application.Services;
public class DateService : IDateService
{
    public string FormatTime(DateTime time)
        => time.ToString("HH:mm:ss.fff");

    public DateTime Now()
        => DateTime.Now;  
}

public interface IDateService : ITransientService
{
    DateTime Now();
    string FormatTime(DateTime time);

}
