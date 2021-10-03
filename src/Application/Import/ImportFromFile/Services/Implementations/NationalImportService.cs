using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Core.Services;
using EnduranceJudge.Domain.Aggregates.Common.Horses;
using System.Collections.Generic;
using System.Linq;
using static EnduranceJudge.Application.ApplicationConstants;

namespace EnduranceJudge.Application.Import.ImportFromFile.Services.Implementations
{
    public class NationalImportService : ExcelServiceBase, INationalImportService
    {
        private readonly IFileService file;

        public NationalImportService(IFileService file)
        {
            this.file = file;
        }

        public IEnumerable<Horse> ImportForNational(string filePath)
        {
            var file = this.file.Get(filePath);
            this.Initialize(file);

            var sheet = this.Excel.Workbook.Worksheets.FirstOrDefault();
            if (sheet == null)
            {
                return null;
            }

            var row = ExcelMaps.ImportNational.FIRST_ENTRY_ROW;
            var horses = new List<Horse>();

            while(true)
            {
                var name = sheet.Cells[row, ExcelMaps.ImportNational.NAME_COLUMN].Text;
                var breed = sheet.Cells[row, ExcelMaps.ImportNational.BREED_COLUMN].Text;
                var feiId = sheet.Cells[row, ExcelMaps.ImportNational.FEI_ID_COLUMN].Text;
                var club = sheet.Cells[row, ExcelMaps.ImportNational.CLUB_COLUMN].Text;

                if (string.IsNullOrWhiteSpace(name)
                    && string.IsNullOrWhiteSpace(breed)
                    && string.IsNullOrWhiteSpace(feiId))
                {
                    break;
                }

                // var horse = this.horseFactory.Create(feiId, name, breed, club);
                // horses.Add(horse);

                row++;
            }

            return horses;
        }
    }
}
