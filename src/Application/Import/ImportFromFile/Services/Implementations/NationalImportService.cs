using EnduranceJudge.Application.Core.Services;
using EnduranceJudge.Application.Factories;
using EnduranceJudge.Application.Import.ImportFromFile.Models;
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
        private readonly IHorseFactory horseFactory;

        public NationalImportService(IFileService file, IHorseFactory horseFactory)
        {
            this.file = file;
            this.horseFactory = horseFactory;
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

            var row = ExcelMaps.ImportNational.FirstEntryRow;
            var result = new List<HorseForNationalImportModel>();

            while(true)
            {
                var name = sheet.Cells[row, ExcelMaps.ImportNational.NameColumn].Text;
                var breed = sheet.Cells[row, ExcelMaps.ImportNational.BreedColumn].Text;
                var feiId = sheet.Cells[row, ExcelMaps.ImportNational.FeiIdColumn].Text;

                if (string.IsNullOrWhiteSpace(name)
                    && string.IsNullOrWhiteSpace(breed)
                    && string.IsNullOrWhiteSpace(feiId))
                {
                    break;
                }

                var horse = new HorseForNationalImportModel(feiId, name, breed);
                result.Add(horse);

                row++;
            }

            var horses = result.Select(this.horseFactory.Create);

            return horses;
        }
    }
}
