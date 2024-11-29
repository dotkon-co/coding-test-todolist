using MediatR;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;

namespace UMBIT.ToDo.BuildingBlocks.Message.Messagem.Interfaces
{
    public interface IUMBITCommandRequestHandler<TCommand> : IRequestHandler<TCommand, UMBITMessageResponse> where TCommand : class, IRequest<UMBITMessageResponse>
    {
    }
    public interface IUMBITCommandRequestHandler<Tcommand, TResponse> : IRequestHandler<Tcommand, UMBITMessageResponse<TResponse>>
        where TResponse : class
        where Tcommand : class, IRequest<UMBITMessageResponse<TResponse>>
    {
    }
}
