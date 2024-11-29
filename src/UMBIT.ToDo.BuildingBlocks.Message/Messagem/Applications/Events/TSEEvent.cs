using MediatR;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;

namespace UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Events
{
    [Serializable]
    public abstract class UMBITEvent : UMBITMensagem, INotification
    {
    }
}
