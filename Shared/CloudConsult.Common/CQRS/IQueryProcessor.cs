using CloudConsult.Common.Builders;
using MediatR;

namespace CloudConsult.Common.CQRS
{
    public interface IQueryProcessor<in TQuery, TQueryResponse> : IRequestHandler<TQuery, IApiResponse<TQueryResponse>>
        where TQuery : IQuery<TQueryResponse>
    {
    }
}