using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Exceptions;
using EnduranceJudge.Application.Imports.Services;
using EnduranceJudge.Core.ConventionalServices;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Domain.Aggregates.Import;
using static EnduranceJudge.Localization.Strings.Application;

namespace EnduranceJudge.Application.Services
{
    public class ImportService : IImportService
    {
        private readonly IStateUpdater stateUpdater;
        private readonly IFileService file;
        private readonly IInternationalReader internationalReader;
        private readonly INationalReader nationalReader;

        public ImportService(
            IStateUpdater stateUpdater,
            IFileService file,
            IInternationalReader internationalReader,
            INationalReader nationalReader)
        {
            this.stateUpdater = stateUpdater;
            this.file = file;
            this.internationalReader = internationalReader;
            this.nationalReader = nationalReader;
        }

        public void Import(string filePath)
        {
            var fileExtension = this.file.GetExtension(filePath);
            if (fileExtension != ApplicationConstants.FileExtensions.Xml && fileExtension != ApplicationConstants.FileExtensions.SupportedExcel)
            {
                var message = string.Format(
                    UNSUPPORTED_IMPORT_FILE_TEMPLATE,
                    ApplicationConstants.FileExtensions.Xml,
                    ApplicationConstants.FileExtensions.SupportedExcel);

                throw new AppException(message);
            }

            ImportManager manager;
            if (fileExtension == ApplicationConstants.FileExtensions.Xml)
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

    public interface IImportService : IService
    {
        void Import(string filePath);
    }
}
