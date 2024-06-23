namespace EMS.Tools.CameraWatcher;

using OpenAI.Chat;
using OpenCvSharp;
using System.ClientModel;
using System.Diagnostics;

public class VideoWatcher
{
    public static async Task Watch()
    {
        var path = "C:/tmp/camera-watcher-video.mp4";

        var capture = new VideoCapture(path);
        if (!capture.Set(VideoCaptureProperties.ConvertRgb, 0))
        {
            Console.WriteLine("Cannot set ConvertRgb");
        }

        if (!capture.IsOpened())
        {
            Console.WriteLine("Could not open the video file.");
            return;
        }

        // Get the frames per second (fps) of the video
        var fps = Math.Round(capture.Fps);
        var frameInterval = fps / (fps / 3);

        Console.WriteLine($"Video FPS: {fps}");
        Console.WriteLine($"Processing every {frameInterval} frames.");

        var groupedFrames = new List<List<Mat>>();
        var frameIndex = 0;

        while (true)
        {
            var frame = new Mat();
            capture.Read(frame);

            if (frame.Empty())
            {
                // End of video
                break;
            }
            
            frame = frame.Resize(new(frame.Width / 2, frame.Height / 2), 0, 0);

            if (frameIndex++ % frameInterval == 0)
            {
                int currentSecond = frameIndex / (int)fps;
                Console.WriteLine($"Processing frame '{frameIndex}' on second '{currentSecond}'");

                // Check if we need to start a new group for the next second
                if (groupedFrames.Count <= currentSecond)
                {
                    groupedFrames.Add([]);
                }

                // Add the frame to the current second's group
                groupedFrames[currentSecond].Add(frame);
                Console.WriteLine($"Processed frame at {currentSecond} second.");
            }
        }

        capture.Release();

        Console.WriteLine("Video processing completed");
        Console.WriteLine($"Seconds in footage: '{groupedFrames.Count}'");
        var second = 0;
        foreach (var framesPerSecond in groupedFrames)
        {
            Console.WriteLine($"{++second}: {framesPerSecond.Count} frames");
        }

        var openAiClient = CreateOpenAIClient();
        Console.WriteLine("Created OpenAI client");

        await Task.WhenAll(groupedFrames.Select(x => SendOpenAIRequest(openAiClient, x)));
    }

    static ChatClient CreateOpenAIClient()
    {
        var credential = new ApiKeyCredential("");
        return new ChatClient("gpt-4o", credential);
    }

    static async Task<string> SendOpenAIRequest(ChatClient client, List<Mat> frames)
    {
        var instruction =
           "You are a system that detects when a Combination (a human rider and it's horse) cross a finish line. " +
           "You will be given a sequence of images (frames spanning 1 second period) and you have to respond in one of 2 ways:" +
           " 1) if no Combinations cross you should respond 'NO'" +
           " 2) if a Combination crosses then you should respond 'YES: <combination-number>'" +
           " where <combination-number> is a placeholder for the Number of the Combination." +
           "The finish line is represented by an arch" +
           " consisting of 2 vertical stands and a horizontal beam that connects them." +
           "A Combination is considered having crossed the finish line the moment" +
           " the horse's head 'touches' the imaginary vertical plane of the finish line." +
           "The Number of the Combination can be seen as a white number on the chest of a vest the human rider is carrying." +
           "You should ignore Combinations that have already crossed the finish line. ";
        
        var headline = ChatMessageContentPart.CreateTextMessageContentPart("Following are the video frames");
        var userMessageParts = new List<ChatMessageContentPart>() { headline };

        Console.WriteLine($"Combining '{frames.Count}' frames in an OpenAI request");
        foreach (var frame in frames)
        {
            var binary = new BinaryData(frame.ToBytes());
            var frameMessage = ChatMessageContentPart.CreateImageMessageContentPart(binary, "image/jpg", new ImageChatMessageContentPartDetail("low"));
            userMessageParts.Add(frameMessage);
        }
        var systemMessage = ChatMessage.CreateSystemMessage(instruction);
        var userMessage = ChatMessage.CreateUserMessage(userMessageParts);
        var messages = new List<ChatMessage> { systemMessage, userMessage };

        Console.WriteLine($"Sending OpenAI request: '{messages.Count}' messages, combined length: {messages.Sum(x => x.Content.ToString().Length)}");

        var stopwatch = new Stopwatch();
        stopwatch.Start();
        var response = await client.CompleteChatAsync(messages);
        stopwatch.Stop();
        var result = response.Value.ToString();
        Console.WriteLine($"Response '{result}' took '{stopwatch.Elapsed}'");
        
        return result;
    }
}
