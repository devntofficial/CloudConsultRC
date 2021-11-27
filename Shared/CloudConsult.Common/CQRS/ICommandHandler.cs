using CloudConsult.Common.Builders;
using MediatR;

namespace CloudConsult.Common.CQRS
{
    public interface ICommandHandler<in TCommand, TCommandResponse> : IRequestHandler<TCommand, IApiResponse<TCommandResponse>>
        where TCommand : ICommand<TCommandResponse>
    {
    }

    public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, IApiResponse> where TCommand : ICommand
    {

    }
}