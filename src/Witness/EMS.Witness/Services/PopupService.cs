using Core.ConventionalServices;
using Core.Domain.AggregateRoots.Manager;

namespace EMS.Witness.Services;

public class PopupService : IPopupService
{
    private const string CLOSE = "Close";

    public async Task<string> RenderActionSheet(string text, params string[] values)
    {
        return await Application.Current.MainPage.DisplayActionSheet("Select Type", CLOSE, null, values);
    }

    public async Task<WitnessEventType?> SelecEventType()
    {
        var result = await this.RenderActionSheet(
            "Select Type",
            WitnessEventType.Arrival.ToString(),
            WitnessEventType.VetIn.ToString());
        if (result == CLOSE)
        {
            return null;
        }
        return Enum.Parse<WitnessEventType>(result);
    }
}

public interface IPopupService : ITransientService
{
    Task<WitnessEventType?> SelecEventType();
    Task<string> RenderActionSheet(string text, params string[] values);
}
