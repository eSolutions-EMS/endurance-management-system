namespace EMS.Tools.CameraWatcher;

using OpenAI.Chat;
using OpenCvSharp;
using System.ClientModel;

public class VideoWatcher
{
    public static async Task Watch()
    {
        var path = "C:/tmp/camera-watcher-video.mp4";

        var capture = new VideoCapture(path);
        if (!capture.IsOpened())
        {
            Console.WriteLine("Could not open the video file.");
            return;
        }

        // Get the frames per second (fps) of the video
        var fps = capture.Fps;
        var frameInterval = (int)(fps / 1);

        Console.WriteLine($"Video FPS: {fps}");
        Console.WriteLine($"Processing every {frameInterval} frames.");

        var groupedFrames = new List<List<Mat>>();
        var frameIndex = 0;
        var secondIndex = 0;

        while (true)
        {
            Mat frame = new Mat();
            capture.Read(frame);

            if (frame.Empty())
            {
                // End of video
                break;
            }

            if (frameIndex % frameInterval == 0)
            {
                int currentSecond = frameIndex / (int)fps;

                // Check if we need to start a new group for the next second
                if (groupedFrames.Count <= currentSecond)
                {
                    groupedFrames.Add(new List<Mat>());
                }

                // Add the frame to the current second's group
                groupedFrames[currentSecond].Add(frame);
                Console.WriteLine($"Processed frame at {currentSecond} second.");
            }

            frameIndex++;
        }

        capture.Release();

        Console.WriteLine("Video processing completed.");

        var base64Frames = await EncodeFramesToBase64(groupedFrames);

        var openAiClient = CreateOpenAIClient();
        Console.WriteLine("Created OpenAI client");

        await Task.WhenAll(base64Frames.Skip(2).Take(1).Select(x => SendOpenAIRequest(openAiClient, x)));
    }

    static ChatClient CreateOpenAIClient()
    {
        var credential = new ApiKeyCredential("");
        return new ChatClient("gpt-4o", credential);
    }

    static async Task<string> SendOpenAIRequest(ChatClient client, List<string> base64Frames)
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
        
        var systemMessage = ChatMessage.CreateSystemMessage(instruction);
        var headline = ChatMessage.CreateUserMessage("Following are the video frames encoded in base64");
        var messages = new List<ChatMessage>() { systemMessage, headline };
        
        foreach (var frame in base64Frames)
        {
            var frameMessage = ChatMessage.CreateUserMessage(frame);
            messages.Add(frameMessage);
        }
        Console.WriteLine($"Sending OpenAI request with '{messages.Count}' messages");
        var response = await client.CompleteChatAsync(messages);
        var result = response.Value.ToString();
        Console.WriteLine(result);
        return result;
    }

    static async Task<List<List<string>>> EncodeFramesToBase64(List<List<Mat>> groupedFrames)
    {
        var base64GroupedFrames = new List<List<string>>();
        foreach (var secondGroup in groupedFrames)
        {
            var tasks = new List<Task<string>>();
            var base64Frames = new List<string>();
            foreach (var frame in secondGroup)
            {
                var task = Task.Run(() => EncodeMatToBase64(frame));
                tasks.Add(task);
            }
            var results = await Task.WhenAll(tasks);
            base64GroupedFrames.Add([.. results]);
        }
        return base64GroupedFrames;
    }

    static Task<string> EncodeMatToBase64(Mat frame)
    {
        Console.WriteLine($"Encoing frame in base64");
        var imageData = frame.ToBytes();
        var result = Convert.ToBase64String(imageData);
        return Task.FromResult(result);
    }
}
