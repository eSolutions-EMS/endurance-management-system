using EnduranceJudge.Application.Core.Exceptions;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Import.Contracts;
using EnduranceJudge.Application.Import.ImportFromFile.Services;
using EnduranceJudge.Core.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static EnduranceJudge.Application.ApplicationConstants;
using static EnduranceJudge.Localization.Strings.Application;

namespace EnduranceJudge.Application.Import.ImportFromFile
{
    public class ImportFromFile : IRequest
    {
        public string FilePath { get; set; }

        public class ImportFromFileHandler : Handler<ImportFromFile>
        {
            private readonly IHorseCommands horseCommands;
            private readonly INationalImportService nationalImport;
            private readonly IInternationalImportService internationalImport;
            private readonly IEnduranceEventCommands enduranceEventCommands;
            private readonly IFileService file;

            public ImportFromFileHandler(
                IHorseCommands horseCommands,
                INationalImportService nationalImport,
                IInternationalImportService internationalImport,
                IEnduranceEventCommands enduranceEventCommands,
                IFileService file)
            {
                this.horseCommands = horseCommands;
                this.nationalImport = nationalImport;
                this.internationalImport = internationalImport;
                this.enduranceEventCommands = enduranceEventCommands;
                this.file = file;
            }

            public override async Task DoHandle(ImportFromFile request, CancellationToken token)
            {
                var filePath = request.FilePath;
                var fileExtension = this.file.GetExtension(filePath);
                if (fileExtension != FileExtensions.Xml && fileExtension != FileExtensions.SupportedExcel)
                {
                    var message = string.Format(
                        UnsupportedImportFileTemplate,
                        FileExtensions.Xml,
                        FileExtensions.SupportedExcel);

                    throw new AppException(message);
                }

                if (fileExtension == FileExtensions.Xml)
                {
                    var enduranceEvent = this.internationalImport.FromInternational(filePath);
                    await this.enduranceEventCommands.Save(enduranceEvent, token);
                }
                else
                {
                    var horses = this.nationalImport.ImportForNational(filePath);
                    await this.horseCommands.Create(horses, token);
                }
            }
        }
    }
}
