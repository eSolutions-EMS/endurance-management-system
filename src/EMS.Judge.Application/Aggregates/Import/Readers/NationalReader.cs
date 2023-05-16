using EMS.Core.Application.Core.Services;
using EMS.Core.ConventionalServices;
using EMS.Core.Services;
using EMS.Core.Domain.AggregateRoots.Import.Models.National;
using System.Linq;
using static EMS.Core.Application.ApplicationConstants;

namespace EMS.Core.Application.Aggregates.Import.Readers;

public class NationalReader : ExcelServiceBase, INationalReader
{
    private readonly IFileService file;

    public NationalReader(IFileService file)
    {
        this.file = file;
    }

    public NationalData Read(string filePath)
    {
        var file = this.file.Get(filePath);
        this.Initialize(file);

        var sheet = this.Excel.Workbook.Worksheets.FirstOrDefault();
        if (sheet == null)
        {
            return null;
        }

        var row = ApplicationConstants.ExcelMaps.ImportNational.FIRST_ENTRY_ROW;
        var data = new NationalData();

        while(true)
        {
            var name = sheet.Cells[row, ApplicationConstants.ExcelMaps.ImportNational.NAME_COLUMN].Text;
            var breed = sheet.Cells[row, ApplicationConstants.ExcelMaps.ImportNational.BREED_COLUMN].Text;
            var feiId = sheet.Cells[row, ApplicationConstants.ExcelMaps.ImportNational.FEI_ID_COLUMN].Text;
            var club = sheet.Cells[row, ApplicationConstants.ExcelMaps.ImportNational.CLUB_COLUMN].Text;

            if (string.IsNullOrWhiteSpace(name)
                && string.IsNullOrWhiteSpace(breed)
                && string.IsNullOrWhiteSpace(feiId)
                && string.IsNullOrWhiteSpace(club))
            {
                break;
            }

            var horse = new HorseExcelSchema
            {
                FeiId = feiId,
                Name = name,
                Breed = breed,
                Club = club,
            };

            data.Horses.Add(horse);

            row++;
        }

        return data;
    }
}

public interface INationalReader : ITransientService
{
    public NationalData Read(string filePath);
}
