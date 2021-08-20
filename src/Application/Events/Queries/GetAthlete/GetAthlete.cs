using EnduranceJudge.Application.Contracts.Countries;
using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Application.Events.Models;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Domain.Aggregates.Common.Athletes;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Queries.GetAthlete
{
    public class  GetAthlete : IdentifiableRequest<AthleteRootModel>
    {
        public class GetAthleteHandler : GetOneHandler<GetAthlete, AthleteRootModel, Athlete>
        {
            private readonly ICountryQueries countryQueries;

            public GetAthleteHandler(IQueriesBase<Athlete> query, ICountryQueries countryQueries) : base(query)
            {
                this.countryQueries = countryQueries;
            }

            public override async Task<AthleteRootModel> Handle(
                GetAthlete request,
                CancellationToken token)
            {
                var athlete = await base.Handle(request, token);
                athlete.Countries = await this.countryQueries.All<CountryListModel>();
                return athlete;
            }
        }
    }
}
