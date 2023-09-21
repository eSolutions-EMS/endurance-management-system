using Common.Conventions;

namespace Common;

public class FilesystemService : IFilesystemService
{
    public Task<string> Read(string path)
    {
        throw new NotImplementedException();
    }

    public Task Write(string path, string content)
    {
        throw new NotImplementedException();
    }
}

public interface IFilesystemService : ITransientService
{
    Task<string> Read(string path);
    Task Write(string path, string content);
}