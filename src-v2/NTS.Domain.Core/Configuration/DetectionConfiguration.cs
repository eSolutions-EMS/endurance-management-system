namespace NTS.Domain.Core.Configuration;

public class DetectionConfiguration
{
    public DetectionConfiguration(DetectionMode mode)
    {
        Mode = mode;
    }

    public DetectionMode Mode { get; set; }
    public bool IsRfid()
    {
        if (Mode.ToString() == "Rfid") 
        {
            return true; 
        }
        return false;
    }

    public bool IsComputerVision() 
    {
        if(Mode.ToString() == "ComputerVision") 
        {
            return true; 
        }
        return false;
    }
}

    public enum DetectionMode
    { 
        Manual = 0,
        Rfid = 1,
        ComputerVision = 2
    }