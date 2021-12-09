using CloudConsult.Common.Builders;
using MediatR;

namespace CloudConsult.Common.CQRS;

public interface IQuery<T> : IRequest<IApiResponse<T>>
{
}

public interface IPaginatedQuery<T> : IRequest<IApiResponse<T>>
{
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}