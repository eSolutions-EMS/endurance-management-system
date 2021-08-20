using EnduranceJudge.Application.Contracts.Countries;
using EnduranceJudge.Application.Core.Handlers;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Queries.GetCountriesList
{

    // TODO: probably remove
    public class GetCountriesList : IRequest<IEnumerable<CountryListModel>>
    {
        public class GetCountriesListingHandler : Handler<GetCountriesList, IEnumerable<CountryListModel>>
        {
            private readonly ICountryQueries countryQueries;

            public GetCountriesListingHandler(ICountryQueries countryQueries)
            {
                this.countryQueries = countryQueries;
            }

            public override async Task<IEnumerable<CountryListModel>> Handle(
                GetCountriesList request,
                CancellationToken token)
            {
                var countries = await this.countryQueries.All<CountryListModel>();
                return countries;
            }
        }
    }
}
