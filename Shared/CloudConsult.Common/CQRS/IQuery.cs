using CloudConsult.Common.Builders;
using MediatR;

namespace CloudConsult.Common.CQRS
{
    public interface IQuery<T> : IRequest<IApiResponse<T>>
    {
    }
}