using Core.Application.Services;
using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager;

namespace EMS.Witness.Services;

public class PopupService : IPopupService
{
    private const string CLOSE = "Close";
    private readonly IDateService dateService;

    public PopupService(IDateService dateService)
    {
        this.dateService = dateService;
    }

    public async Task<DateTime?> EditTime(DateTime time)
    {
        var formatted = this.dateService.FormatTime(time, showMs: true);
        var resultString = await Application.Current.MainPage.DisplayPromptAsync(
            "Edit time",
            "Keep the current time format",
            initialValue: formatted,
            keyboard: Keyboard.Numeric
        );
        if (resultString == null)
        {
            return null;
        }
        return this.dateService.FromString(resultString);
    }

    public async Task<string> RenderActionSheet(string text, params string[] values)
    {
        return await Application.Current.MainPage.DisplayActionSheet(
            "Select Type",
            CLOSE,
            null,
            values
        );
    }

    public async Task<WitnessEventType?> SelecEventType()
    {
        var result = await this.RenderActionSheet(
            "Select Type",
            WitnessEventType.Arrival.ToString(),
            WitnessEventType.VetIn.ToString()
        );
        if (result == CLOSE)
        {
            return null;
        }
        return Enum.Parse<WitnessEventType>(result);
    }
}

public interface IPopupService : ITransientService
{
    Task<DateTime?> EditTime(DateTime time);
    Task<WitnessEventType?> SelecEventType();
    Task<string> RenderActionSheet(string text, params string[] values);
}
