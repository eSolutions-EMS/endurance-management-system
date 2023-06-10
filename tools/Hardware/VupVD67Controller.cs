using System.Text;
using Vup.reader;

namespace EMS.Tools.Hardware;

public class VupVD67Controller
{
    private VD67Reader reader = null!;
    
    public VupVD67Controller()
    {
        this.reader = new VD67Reader();
    }

    public bool IsReading { get; private set; }
    public bool IsWriting { get; private set; }
    
    public void Connect()
    {
        var ants = this.reader.GetAntCount();
        var result = this.reader.Connect();
        if (!result.Success)
        {
            Console.WriteLine($"Error '{result.ErrorCode}': {result.Message}");
        }
        Console.WriteLine("Success");
    }

    public async Task StartReading()
    {
        this.IsReading = true;
        this.IsWriting = false;
        while (this.IsReading)
        {
            try
            {
                var empty = Array.Empty<byte>();
                var result = this.reader.Read6C(memory_bank.memory_bank_epc, default, default, empty, empty);
                if (result.Success)
                {
                    Console.WriteLine($"Read: {Encoding.UTF8.GetString(result.Result)}");
                }
                else
                {
                    Console.WriteLine($"Fail {result.ErrorCode}: {result.Message}");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message + Environment.NewLine + exception.StackTrace}");
            }
            
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
    
    public async Task StartWriting(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        this.IsReading = false;
        this.IsWriting = true;
        while (this.IsWriting)
        {
            try
            {
                var result = this.reader.Write6C(memory_bank.memory_bank_epc, default, default, bytes, default);
                if (result.Success)
                {
                    Console.WriteLine($"Write success");
                }
                else
                {
                    Console.WriteLine($"Fail {result.ErrorCode}: {result.Message}");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error: {exception.Message + Environment.NewLine + exception.StackTrace}");
            }
            
            await Task.Delay(TimeSpan.FromSeconds(5));
        }
    }
}
