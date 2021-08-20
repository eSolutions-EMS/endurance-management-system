namespace EnduranceJudge.Application.Import.ImportFromFile.Models
{
    public class HorseForNationalImportModel
    {
        private HorseForNationalImportModel()
        {
        }

        public HorseForNationalImportModel(string feiId, string name, string breed)
        {
            this.FeiId = feiId;
            this.Name = name;
            this.Breed = breed;
        }

        public string FeiId { get; }

        public string Name { get; }

        public string Breed { get; }
    }
}
