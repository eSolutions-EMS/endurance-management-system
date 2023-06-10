namespace EMS.Tools.Hardware;

public static class Hardware
{
    public static async Task Run()
    {
        var controller = new VupVD67Controller();

        controller.Connect();
        await controller.StartReading();
    }
}
