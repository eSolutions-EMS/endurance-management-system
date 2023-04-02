using Prism.Mvvm;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Hardware.Tags;

public class TagViewModel : BindableBase
{
    private string id;
    private int detectedCount;

    public string Id
    {
        get => this.id;
        set => this.SetProperty(ref this.id, value);
    }
    public int DetectedCount
    {
        get => this.detectedCount; 
        set => this.SetProperty(ref this.detectedCount, value);
    }
}
