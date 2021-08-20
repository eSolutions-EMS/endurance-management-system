using EnduranceJudge.Core.ConventionalServices;
using System.IO;
using System.Threading.Tasks;

namespace EnduranceJudge.Core.Services
{
    public interface IFileService : IService
    {
        FileInfo Get(string path);

        bool Exists(string path);

        public Task Create(string filePath, string content);

        public Task<string> Read(string name);

        StreamReader ReadStream(string filePath);

        string GetExtension(string path);
    }
}
