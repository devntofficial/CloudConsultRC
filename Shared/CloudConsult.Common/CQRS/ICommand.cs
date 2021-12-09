using CloudConsult.Common.Builders;
using MediatR;

namespace CloudConsult.Common.CQRS;

public interface ICommand<T> : IRequest<IApiResponse<T>>
{

}

public interface ICommand : IRequest<IApiResponse>
{

}