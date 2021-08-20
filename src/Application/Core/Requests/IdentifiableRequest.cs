using EnduranceJudge.Core.Models;
using MediatR;

namespace EnduranceJudge.Application.Core.Requests
{
    public class IdentifiableRequest : IdentifiableRequestBase, IRequest
    {
    }

    public class IdentifiableRequest<T> : IdentifiableRequestBase, IRequest<T>
    {
    }

    public abstract class IdentifiableRequestBase : IReIdentifiable
    {
        public bool Equals(IIdentifiable identifiable)
        {
            if (ReferenceEquals(identifiable, null))
            {
                return false;
            }

            if (this.GetType() != identifiable.GetType())
            {
                return false;
            }

            if (this.Id != default &&  identifiable.Id != default)
            {
                return this.Id == identifiable.Id;
            }

            return false;
        }

        public int Id { get; set; }
    }
}
