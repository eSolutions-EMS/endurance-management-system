using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EnduranceJudge.Gateways.Desktop.Views.Content.Hardware.Tags;

public class TagViewModel : INotifyPropertyChanged
{
    private string _id;
    private int _detectedCount;

    public string Id
    {
        get => this._id;
        set => this.SetField(ref this._id, value);
    }
    public int DetectedCount
    {
        get => this._detectedCount; 
        set => this.SetField(ref this._detectedCount, value);
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
