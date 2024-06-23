using OpenCvSharp;

namespace EMS.Tools.CameraWatcher;

public class VideoCaptureSettings
{
    public static void Test()
    {
        var path = "C:/tmp/camera-watcher-video.mp4";

        var baselineCapture = Capture(path, _ => { });
        var bitrate = baselineCapture.Get(VideoCaptureProperties.BitRate);
        var height = baselineCapture.Get(VideoCaptureProperties.FrameHeight);
        var width = baselineCapture.Get(VideoCaptureProperties.FrameWidth);
        var fourCC = baselineCapture.Get(VideoCaptureProperties.FourCC);
        var format = baselineCapture.Get(VideoCaptureProperties.Format);
        var convertRgb = baselineCapture.Get(VideoCaptureProperties.ConvertRgb);

        
        var bitrateCapture = Capture(path, ConfigureBitrate);
        var bitrateCaptureBitRate = bitrateCapture.Get(VideoCaptureProperties.BitRate);

        var heightCapture = Capture(path, ConfigureHeight);
        var heightCaptureHeight = heightCapture.Get(VideoCaptureProperties.FrameHeight);

        var widthCapture = Capture(path, ConfigureWidth);
        var widthCaptureWidth = widthCapture.Get(VideoCaptureProperties.FrameWidth);

        var fourCCCapture = Capture(path, ConfigureFourCC);
        var fourCCCaptureFourCC = fourCCCapture.Get(VideoCaptureProperties.FourCC);

        var formatCapture = Capture(path, ConfigureFormat);
        var formatCaptureFormat = formatCapture.Get(VideoCaptureProperties.Format);

        var convertRgbCapture = Capture(path, DisableConvertRgb);
        var convertRgbCaptureConvertRgb = convertRgbCapture.Get(VideoCaptureProperties.ConvertRgb);

        var resizeCapture = Capture(path);

        for (var i = 0; i < 4; i++)
        {
            var baselineFrame = ReadFrame(baselineCapture);
            var bitrateFrame = ReadFrame(bitrateCapture);
            var heightFrame = ReadFrame(heightCapture);
            var widthFrame = ReadFrame(widthCapture);
            var fourCCFrame = ReadFrame(fourCCCapture);
            var formatFrame = ReadFrame(formatCapture);
            var convertRgbFrame = ReadFrame(convertRgbCapture);
            var resizeFrame = ReadFrame(resizeCapture);
            resizeFrame = resizeFrame.Resize(new(baselineFrame.Width / 2, baselineFrame.Height / 2), 0, 0);
            var resizeAndRgbFrame = convertRgbFrame.Resize(new(baselineFrame.Width / 2, baselineFrame.Height / 2), 0, 0);

            CompareFrames(baselineFrame, bitrateFrame, heightFrame, widthFrame, fourCCFrame, formatFrame, convertRgbFrame, resizeFrame, resizeAndRgbFrame);
        }
    }

    private static void CompareFrames(params Mat[] frames)
    {
        var baselineSize = MeasureFrame(frames[0]);
        var bitrateSize = MeasureFrame(frames[1]);
        var heightSize = MeasureFrame(frames[2]);
        var widthSize = MeasureFrame(frames[3]);
        var fourCCSize = MeasureFrame(frames[4]);
        var formatSize = MeasureFrame(frames[5]);
        var convertRgbSize = MeasureFrame(frames[6]);
        var resizeSize = MeasureFrame(frames[7]);
        var resizeAndRgbSize = MeasureFrame(frames[8]);

        Console.WriteLine($"Baseline    : {baselineSize}");
        Console.WriteLine($"Bitrate     : {bitrateSize}  |  {EvaluateSize(baselineSize, bitrateSize)}");
        Console.WriteLine($"Width       : {widthSize}  |  {EvaluateSize(baselineSize, widthSize)}");
        Console.WriteLine($"Height      : {heightSize}  |  {EvaluateSize(baselineSize, heightSize)}");
        Console.WriteLine($"FourCC      : {fourCCSize}  |  {EvaluateSize(baselineSize, fourCCSize)}");
        Console.WriteLine($"Format      : {formatSize}  |  {EvaluateSize(baselineSize, formatSize)}");
        Console.WriteLine($"ConvertRGB  : {convertRgbSize}  |  {EvaluateSize(baselineSize, convertRgbSize)}");
        Console.WriteLine($"Resize      : {resizeSize}  |  {EvaluateSize(baselineSize, resizeSize)}");
        Console.WriteLine($"Resize + Rgb: {resizeAndRgbSize}  |  {EvaluateSize(baselineSize, resizeAndRgbSize)}");
    }

    private static string EvaluateSize(int baselineSize, int otherSize)
    {
        if (otherSize == baselineSize)
        {
            return "EQUAL";
        }
        return otherSize > baselineSize
            ? $"+ {otherSize - baselineSize}"
            : $"- {baselineSize - otherSize}";
    }

    private static int MeasureFrame(Mat frame)
    {
        return frame.ToBytes().Length;
    }

    private static Mat ReadFrame(VideoCapture capture)
    {
        var frame = new Mat();
        capture.Read(frame);
        return frame;
    }

    private static VideoCapture Capture(string path, Action<VideoCapture>? configure = null)
    {
        var capture = new VideoCapture(path);
        configure?.Invoke(capture);
        return capture;
    }

    private static void ConfigureBitrate(VideoCapture capture)
    {
        var bitrate = capture.Get(VideoCaptureProperties.BitRate);
        if (!capture.Set(VideoCaptureProperties.BitRate, bitrate / 2))
        {
            Throw("Bitrate");
        }
    }

    private static void ConfigureHeight(VideoCapture capture)
    {
        var height = capture.Get(VideoCaptureProperties.FrameHeight);
        if (!capture.Set(VideoCaptureProperties.FrameHeight, height / 2))
        {
            Throw("FrameHeight");
        }
    }

    private static void ConfigureWidth(VideoCapture capture)
    {
        var width = capture.Get(VideoCaptureProperties.FrameWidth);
        if (!capture.Set(VideoCaptureProperties.FrameWidth, width / 2))
        {
            Throw("FrameWidth");
        }
    }

    private static void ConfigureFourCC(VideoCapture capture)
    {
        if (!capture.Set(VideoCaptureProperties.FourCC, VideoWriter.FourCC('M', 'J', 'P', 'G')))
        {
            Throw("FourCC");
        }
    }

    private static void ConfigureFormat(VideoCapture capture)
    {
        if (!capture.Set(VideoCaptureProperties.Format, (int)MatType.CV_8UC3))
        {
            Throw("Format");
        }
    }

    private static void DisableConvertRgb(VideoCapture capture)
    {
        if (!capture.Set(VideoCaptureProperties.ConvertRgb, 0))
        {
            Throw("ConvertRgb");
        }
    }

    private static void Throw(string property)
    {
        Console.WriteLine($"{property} configuration not supported");
    }
}
