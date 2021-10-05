using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Imports.Contracts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Imports.Commands
{
    public class SelectWorkFile : IRequest<bool>
    {
        public string DirectoryPath { get; set; }

        public class SelectWorkFileHandler : Handler<SelectWorkFile, bool>
        {
            private readonly IStorageInitializer workFile;

            public SelectWorkFileHandler(IStorageInitializer workFile)
            {
                this.workFile = workFile;
            }

            public override async Task<bool> Handle(SelectWorkFile request, CancellationToken token)
            {
                var workFilePath = $"{request.DirectoryPath}\\{ApplicationConstants.STORAGE_FILE_NAME}";

                var isNewFileCreated = await this.workFile.Initialize(workFilePath, token);

                return isNewFileCreated;
            }
        }
    }
}
