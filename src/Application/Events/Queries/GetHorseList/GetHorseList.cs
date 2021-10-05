using EnduranceJudge.Application.Core.Handlers;
using EnduranceJudge.Application.Events.Common;
using EnduranceJudge.Domain.State.Horses;
using MediatR;
using System.Collections.Generic;

namespace EnduranceJudge.Application.Events.Queries.GetHorseList
{
    public class GetHorseList : IRequest<IEnumerable<ListItemModel>>, IHorseState
    {
        public int Id { get; set; }
        public string FeiId { get; set; }
        public string Name { get; set; }
        public string Club { get; set; }
        public bool IsStallion { get; set; }
        public string Breed { get; set; }
        public string TrainerFeiId { get; set; }
        public string TrainerFirstName { get; set; }
        public string TrainerLastName { get; set; }

        public class GetHorseListHandler : GetAllHandler<GetHorseList, ListItemModel, Horse>
        {
            public GetHorseListHandler() : base()
            {
            }
        }
    }
}
