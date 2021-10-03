using EnduranceJudge.Application.Contracts;
using EnduranceJudge.Application.Core.Contracts;
using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Core.Requests;
using EnduranceJudge.Application.Events.Queries.GetCountriesList;
using EnduranceJudge.Domain.State;
using System.Threading;
using System.Threading.Tasks;

namespace EnduranceJudge.Application.Events.Queries.GetEvent
{
    public class GetEnduranceEvent : IdentifiableRequest<EventRootModel>
    {
        public class GetEnduranceEventHandler
            : GetOneHandler<GetEnduranceEvent, EventRootModel, EventState>
        {
            private readonly ICountryQueries countryQueries;

            public GetEnduranceEventHandler(IQueries<EventState> query, ICountryQueries countryQueries)
                : base(query)
            {
                this.countryQueries = countryQueries;
            }

            public override async Task<EventRootModel> Handle(
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
