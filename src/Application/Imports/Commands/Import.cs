using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Exceptions;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Imports.Services;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Domain.Aggregates.Import;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using static EnduranceJudge.Application.ApplicationConstants;
using static EnduranceJudge.Localization.Strings.Application;

namespace EnduranceJudge.Application.Imports.Commands
{
    public class Import : IRequest
    {
        public string FilePath { get; init; }

        public class ImportHandler : Handler<Import>
        {
            private readonly IStateUpdater stateUpdater;
            private readonly INationalReader nationalReader;
            private readonly IInternationalReader internationalReader;
            private readonly IFileService file;

            public ImportHandler(
                IStateUpdater stateUpdater,
                INationalReader nationalReader,
                IInternationalReader internationalReader,
                IFileService file)
            {
                this.stateUpdater = stateUpdater;
                this.nationalReader = nationalReader;
                this.internationalReader = internationalReader;
                this.file = file;
            }

            public override async Task DoHandle(Import request, CancellationToken token)
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

                ImportManager manager;
                if (fileExtension == FileExtensions.Xml)
                {
                    var data = this.internationalReader.Read(filePath);

                    manager = new ImportManager(data.Event?.Name, data.Event?.CountryNOC);
                    manager.AddCompetitions(data.Competitions);
                    manager.AddAthletes(data.Athletes);
                    manager.AddHorses(data.Horses);
                    manager.AddParticipants(data.Participants);
                }
                else
                {
                    manager = new ImportManager();
                    var horses = this.nationalReader.Read(filePath);
                    foreach (var horse in horses)
                    {
                        manager.AddHorse(horse.Name, horse.Breed, horse.FeiId, horse.Club);
                    }
                }

                this.stateUpdater.Update(manager);
            }
        }
    }
}
