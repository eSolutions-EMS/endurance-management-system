namespace EMS.Witness.Shared.Toasts;

public class Toast
{
    private readonly DateTimeOffset posted;

    public Toast(string title, string? message, UiColor color, int secondsToLive = 5)
    {
        this.Title = title;
        this.Message = message ?? string.Empty;
        this.Color = color;
        this.TimeToBurn = DateTimeOffset.Now.AddSeconds(secondsToLive);
        this.posted = DateTimeOffset.Now;
        this.Id = Guid.NewGuid();
    }

    public Guid Id;
    public string Title { get; }
    public string Message { get; }
    public UiColor Color { get; }
    public DateTimeOffset TimeToBurn { get; }

    public bool IsBurnt => TimeToBurn < DateTimeOffset.Now;
    public string ElapsedTimeText => $"{-this.ElapsedTime.Seconds} seconds ago";

    private TimeSpan ElapsedTime => this.posted - DateTimeOffset.Now;
}

public enum UiColor
{
    Primary,
    Secondary,
    Success,
    Danger,
    Warning,
}
