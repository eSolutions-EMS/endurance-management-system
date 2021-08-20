using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Import.Contracts;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Import.WorkFile
{
    public class SelectWorkFile : IRequest<bool>
    {
        public string DirectoryPath { get; set; }

        public class SelectWorkFileHandler : Handler<SelectWorkFile, bool>
        {
            private readonly IWorkFileService workFile;

            public SelectWorkFileHandler(IWorkFileService workFile)
            {
                this.workFile = workFile;
            }

            public override async Task<bool> Handle(SelectWorkFile request, CancellationToken token)
            {
                var workFilePath = $"{request.DirectoryPath}\\{ApplicationConstants.WorkFileName}";

                var isNewFileCreated = await this.workFile.Initialize(workFilePath, token);

                return isNewFileCreated;
            }
        }
    }
}
