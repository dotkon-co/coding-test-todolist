using UMBIT.ToDo.BuildingBlocks.Message.Messagem;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Events;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;

namespace UMBIT.ToDo.BuildingBlocks.Message.Bus.MediatorBus
{
    public interface IMediatorBus
    {
        void PublicarEvento<TEvent>(TEvent evento)
            where TEvent : UMBITEvent;

        Task<UMBITMessageResponse> EnviarComando<TCommand>(TCommand comando)
            where TCommand : class, IUMBITCommandRequest<TCommand>;

        Task<UMBITMessageResponse<TResp>> EnviarComando<TCommand, TResp>(TCommand comando)
            where TCommand : class, IUMBITCommandRequest<TCommand, TResp>
            where TResp : class;

        Task<UMBITMessageResponse<TResp>> EnviarQuery<TQuery, TResp>(TQuery comando)
            where TQuery : IUMBITQuery<TResp>
            where TResp : class;
    }
}
