using EMS.Core.Application.Aggregates.Import.Readers;
using EMS.Core.Application.Core.Exceptions;
using EMS.Core.Application.Services;
using EMS.Core.ConventionalServices;
using EMS.Core.Services;
using EMS.Core.Domain.AggregateRoots.Import;
using static EMS.Core.Localization.Strings;

namespace EMS.Core.Application.Aggregates.Import;

public class ImportService : IImportService
{
    private readonly IPersistence persistence;
    private readonly IFileService file;
    private readonly IInternationalReader internationalReader;
    private readonly INationalReader nationalReader;

    public ImportService(
        IPersistence persistence,
        IFileService file,
        IInternationalReader internationalReader,
        INationalReader nationalReader)
    {
        this.persistence = persistence;
        this.file = file;
        this.internationalReader = internationalReader;
        this.nationalReader = nationalReader;
    }

    public void Import(string filePath)
    {
        var fileExtension = this.file.GetExtension(filePath);
        if (fileExtension != ApplicationConstants.FileExtensions.Xml
            && fileExtension != ApplicationConstants.FileExtensions.SupportedExcel)
        {
            var message = string.Format(
                UNSUPPORTED_IMPORT_FILE_MESSAGE,
                ApplicationConstants.FileExtensions.Xml,
                ApplicationConstants.FileExtensions.SupportedExcel);

            throw new AppException(message);
        }

        if (fileExtension == ApplicationConstants.FileExtensions.Xml)
        {
            var data = this.internationalReader.Read(filePath);
            var manager = new ImportRoot();
            manager.Import(data);
        }
        else
        {
            var data = this.nationalReader.Read(filePath);
            var manager = new ImportRoot();
            manager.Import(data);
        }

        this.persistence.SaveState();
    }
}

public interface IImportService : ITransientService
{
    void Import(string filePath);
}
