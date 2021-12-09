using CloudConsult.Common.Builders;
using MediatR;

namespace CloudConsult.Common.CQRS;

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, IApiResponse<TResponse>>
    where TCommand : ICommand<TResponse>
{
}

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, IApiResponse> where TCommand : ICommand
{

}