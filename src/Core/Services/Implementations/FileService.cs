using System;
using System.IO;
using System.Threading.Tasks;

namespace EnduranceJudge.Core.Services.Implementations
{
    public class FileService : IFileService
    {
        public FileInfo Get(string path)
            => new FileInfo(path);

        public bool Exists(string path)
            => File.Exists(path);

        public async Task Create(string filePath, string content)
        {
            await using var stream = new StreamWriter(filePath);
            await stream.WriteAsync(content);
        }

        public async Task<string> Read(string filePath)
        {
            using var stream = this.ReadStream(filePath);
            return await stream.ReadToEndAsync();
        }

        public StreamReader ReadStream(string filePath)
        {
            if (!this.Exists(filePath))
            {
                var message = $"File '{filePath}' does not exist.";
                throw new InvalidOperationException(message);
            }

            try
            {
                var stream = new StreamReader(filePath);
                return stream;
            }
            catch (IOException e) when ((e.HResult & 0x0000FFFF) == 32)
            {
                var message = $"Cannot read '{filePath}', because the file is open in another program.";
                throw new InvalidOperationException(message);
            }
        }

        public string GetExtension(string path)
            => Path.GetExtension(path);
    }
}
