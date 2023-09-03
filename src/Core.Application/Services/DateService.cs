using Core.ConventionalServices;
using System;
using System.Linq;

namespace Core.Application.Services;

public class DateService : IDateService
{
    private readonly INotificationService notification;

    public DateService(INotificationService notification)
    {
        this.notification = notification;
    }

    public string FormatTime(DateTime time, bool showMs = false)
        => showMs
            ? time.ToString("HH:mm:ss.fff")
            : time.ToString("HH:mm:ss");

	public string FormatTime(TimeSpan time, bool showMs = false)
		=> showMs
			? time.ToString(@"hh\:mm\:ss\.fff")
            : time.ToString(@"hh\:mm\:ss");

    public DateTime? FromString(string date)
    {
        var components = date
            .Replace(".", ":")
            .Split(":")
            .Where(x => int.TryParse(x, out var _))
            .ToList();
        if (components.Count != 4)
        {
            this.notification.Error("Invalid date/time format");
            return null;
        }
        var numters = components.Select(int.Parse).ToList();
        var hour = numters[0];
        var minute = numters[1];
        var second = numters[2];
        var mil = numters[3];
        return DateTime.Today
            .AddHours(hour)
            .AddMinutes(minute)
            .AddSeconds(second)
            .AddMilliseconds(mil);
    }

    public DateTime Now()
        => DateTime.Now;  
}

public interface IDateService : ITransientService
{
    DateTime Now();
    DateTime? FromString(string date);
    string FormatTime(DateTime time, bool showMs = false);
    string FormatTime(TimeSpan time, bool showMs = false);

}
