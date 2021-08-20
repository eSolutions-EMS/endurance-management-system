using EnduranceJudge.Gateways.Persistence.Entities;
using EnduranceJudge.Gateways.Persistence.Entities.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EnduranceJudge.Gateways.Persistence.Services
{
    public class SeederService : ISeederService
    {
        private readonly EnduranceJudgeDbContext dbContext;

        public SeederService(EnduranceJudgeDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task Seed()
        {
            await this.SeedCountries();
        }

        private async Task SeedCountries()
        {
            var countries = new List<CountryEntity>
            {
                new CountryEntity
                {
                    Name = "Bulgaria",
                    IsoCode = "BUL"
                },
            };

            await this.dbContext.AddRangeAsync(countries);
            await this.dbContext.SaveChangesAsync();
        }
    }
}
