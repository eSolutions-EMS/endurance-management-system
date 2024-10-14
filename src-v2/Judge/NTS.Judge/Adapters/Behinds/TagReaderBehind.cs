using Not.Blazor.Ports.Behinds;
using NTS.Domain;
using NTS.Domain.Core.Configuration;
using NTS.Domain.Objects;
using NTS.Judge.Blazor.Ports;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;

namespace NTS.Judge.Adapters.Behinds;

public class TagReaderBehind : ObservableBehind, ITagRead
{
    private ConcurrentQueue<string> _outputQueue = new ConcurrentQueue<string>();
    private ConcurrentQueue<string> _errorQueue = new ConcurrentQueue<string>();
    private CancellationTokenSource _cancellationTokenSource  = new CancellationTokenSource();
    private System.Timers.Timer _timer = default!;
    private List<RfidSnapshot> _detectedTags { get; set; } = new List<RfidSnapshot>();
    private List<string> _outputMessages { get; set; } = new List<string>();
    private Task? NativeProcess;
    protected override async Task<bool> PerformInitialization()
    {
        NativeProcess = InitializeNativeProcess();
        return _detectedTags.Any();
    }

    public async Task InitializeNativeProcess() { 
        string path = String.Empty;
#if DEBUG
        path = @"C:\Work\not-timing-system\src-v2\NTS.TagRfidControllers\bin\Debug\net8.0\NTS.TagRfidControllers.exe";
#else
        path = Environment.CurrentDirectory + "\\" + "NTS.TagRfidControllers.exe";        
#endif
        ProcessStartInfo processStartInfo = new ProcessStartInfo()
        {
            FileName = path,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        try
        {
            using (Process? process = Process.Start(processStartInfo))
            {
                if(process == null) 
                {
                    _errorQueue.Append("Process didn't start correctly.");
                }
                process.EnableRaisingEvents = true;
                process.OutputDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        _outputQueue.Enqueue(e.Data);
                        Debug.WriteLine($"Output: {e.Data}");
                    }
                };

                process.ErrorDataReceived += (sender, e) =>
                {
                    if (!string.IsNullOrEmpty(e.Data))
                    {
                        _errorQueue.Enqueue(e.Data);
                        Debug.WriteLine($"Error: {e.Data}");
                    }
                };
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                CancellationToken cancellationToken = _cancellationTokenSource.Token;
                await process.WaitForExitAsync(cancellationToken);
            }
        }
        catch (Exception ex) 
        {
            _errorQueue.Append(ex.ToString());
        }
    }

    public void ReadTags(bool enabled)
    {
        _timer = new System.Timers.Timer(1000);
        _timer.Elapsed += ParseOutput; 
        _timer.Interval = 2000; 
        _timer.AutoReset = true; // Keeps the timer firing every interval
        _timer.Enabled = enabled;
    }

    public List<RfidSnapshot> GetSnapshots()
    {
        return _detectedTags;
    }

    public void TagWasProcessed(RfidSnapshot snapshot)
    {
        _detectedTags.Remove(snapshot);
    }

    public void ParseOutput(object? sender, System.Timers.ElapsedEventArgs e)
    {
        while (_outputQueue.TryDequeue(out string output))
        {
            if (output != null && output.Contains("Tag Detected"))
            {
                // expected format of parsed output is:
                // Tag Detected| int tag_number | DateTime.ToString("yyyy-MM-dd HH:mm:ss")
                var tokenized_string = output.Split("|");
                var number = Int32.Parse(tokenized_string[1]);
                CultureInfo provider = CultureInfo.InvariantCulture;
                var date = DateTime.ParseExact(tokenized_string[2].Trim(), "yyyy-MM-dd HH:mm:ss", provider);
                var timestamp = new Timestamp(date);
                var snapshot = new RfidSnapshot(number, StaticOptions.RfidSnapshotType(), timestamp);
                if (!_detectedTags.Contains(snapshot))
                {
                    _detectedTags.Add(snapshot);
                    EmitChange();
                }
            }
            else if(output!= null)
            {
                _outputMessages.Add(output);
                EmitChange();
            }
        }
        while(_errorQueue.TryDequeue(out string error))
        {
            if (error != null)
            {
                _outputMessages.Add(error);
                EmitChange();
            }
        };
    }

    public List<string> GetOutputMessages()
    {
        return _outputMessages;
    }

    public bool IsProcessRunning() 
    {
        return Process.GetProcessesByName("NTS.TagRfidControllers").Any();
    }

    public void Dispose()
    {
        _timer.Stop();
        _timer.Dispose();
        _cancellationTokenSource?.Cancel();
        foreach (var consoleApp in Process.GetProcessesByName("NTS.TagRfidControllers"))
        {
            consoleApp.Kill();
        }
    }

    ~TagReaderBehind()
    {
        Dispose();
    }
}
