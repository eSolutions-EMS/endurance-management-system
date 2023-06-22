using System.Collections.Generic;
using System;
using System.Text;
using System.Threading.Tasks;
using Vup.reader;

namespace EMS.Judge.Application.Hardware;

public class VupVD67Controller
{
    private const int NO_TAG_ERROR_CODE = 24;
    private const char EMPTY_CHAR = '0';
    private const int NUMBER_LENGTH = 3;
    private const int POSITION_LENGTH = 6;
    private VD67Reader reader = null!;
    private TimeSpan throttle;

    public VupVD67Controller(TimeSpan? throttle = null)
    {
        this.reader = new VD67Reader();
        this.throttle = throttle ?? TimeSpan.FromSeconds(1);
    }

    public bool IsReading { get; private set; }
    public bool IsWriting { get; private set; }

    public void Connect()
    {
        var connectionResult = this.reader.Connect();
        var setPowerResult = this.reader.SetAntPower(0, 20);
        if (!connectionResult.Success || !setPowerResult.Success)
        {
            this.WriteLine($"Connect failed '{connectionResult.ErrorCode}': '{connectionResult.Message}'");
        }
        this.WriteLine("Connecction Successful");
    }

    public async Task<string> Read()
    {
        this.IsReading = true;
        this.IsWriting = false;
        while (this.IsReading)
        {
            string data;
            var result = this.reader.Read6C(memory_bank.memory_bank_epc, 4, 12, Array.Empty<byte>(), Convert.FromHexString("00000000")); ;
            if (!result.Success)
            {
                if (result.ErrorCode != NO_TAG_ERROR_CODE)
                {
                    this.WriteLine($"Read Failed '{result.ErrorCode}': {result.Message}");
                }
            }
            else
            {
                data = Encoding.UTF8.GetString(result.Result);
                this.WriteLine($"Read: '{data}'");
                this.IsReading = false;
                return data;
            }

            await Task.Delay(throttle);
        }

        return null;
    }

    public async IAsyncEnumerable<string> StartReading()
    {
        this.IsReading = true;
        this.IsWriting = false;
        while (this.IsReading)
        {
            string data;
            var result = this.reader.Read6C(memory_bank.memory_bank_epc, 4, 16, Array.Empty<byte>(), Convert.FromHexString("00000000")); ;
            if (!result.Success)
            {
                if (result.ErrorCode != NO_TAG_ERROR_CODE)
                {
                    this.WriteLine($"Read Failed '{result.ErrorCode}': {result.Message}");
                }
            }
            else
            {
                data = Encoding.UTF8.GetString(result.Result);
                this.WriteLine($"Read: '{data}'");
                yield return data;
            }

            await Task.Delay(throttle);
        }
    }

    public void StopReading()
    {
        this.IsReading = false;
    }

    public async Task<string> Write(string data)
    {
        if (data.Length != 12)
        {
            throw new Exception("Tag data length must be exactly 12 symbols");
        }
        this.IsReading = false;
        this.IsWriting = true;
        while (this.IsWriting)
        {
            try
            {
                var bytes = Encoding.UTF8.GetBytes(data);
                var result = this.reader.Write6C(memory_bank.memory_bank_epc, 0, Array.Empty<byte>(), bytes, Convert.FromHexString("00000000"));
                if (!result.Success)
                {
                    if (result.ErrorCode != NO_TAG_ERROR_CODE)
                    {
                        this.WriteLine($"Read Failed '{result.ErrorCode}': {result.Message}");
                    }
                }
                else
                {
                    this.WriteLine($"Write Successful: '{data}'");
                    this.IsWriting = false;
                    return data;
                }
            }
            catch (Exception exception)
            {
                throw new Exception($"Write ERROR: {exception.Message + Environment.NewLine + exception.StackTrace}");
            }

            await Task.Delay(this.throttle);
        }
        return null!;
    }

    private void WriteLine(string text)
    {
        Console.WriteLine($"VD67 {text}");
    }
}
