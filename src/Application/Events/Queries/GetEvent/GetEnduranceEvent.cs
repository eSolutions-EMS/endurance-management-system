using EnduranceJudge.Application.Contracts.Countries;
using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Domain.Aggregates.Event.EnduranceEvents;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Queries.GetEvent
{
    public class GetEnduranceEvent : IdentifiableRequest<EnduranceEventRootModel>
    {
        public class GetEnduranceEventHandler
            : GetOneHandler<GetEnduranceEvent, EnduranceEventRootModel, EnduranceEvent>
        {
            private readonly ICountryQueries countryQueries;

            public GetEnduranceEventHandler(IQueriesBase<EnduranceEvent> query, ICountryQueries countryQueries)
                : base(query)
            {
                this.countryQueries = countryQueries;
            }

            public override async Task<EnduranceEventRootModel> Handle(
                GetEnduranceEvent request,
                CancellationToken token)
            {
                var enduranceEvent = await base.Handle(request, token);
                var countries = await this.countryQueries.All<CountryListModel>();
                enduranceEvent.Countries = countries;
                return enduranceEvent;
            }
        }
    }
}
