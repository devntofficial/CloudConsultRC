using CloudConsult.Common.Builders;
using MediatR;

namespace CloudConsult.Common.CQRS;

public interface IQueryProcessor<in TQuery, TResponse> : IRequestHandler<TQuery, IApiResponse<TResponse>>
        where TQuery : IQuery<TResponse>
{
}

public interface IPaginatedQueryProcessor<in TQuery, TResponse> : IRequestHandler<TQuery, IApiResponse<TResponse>>
        where TQuery : IPaginatedQuery<TResponse>
{
}