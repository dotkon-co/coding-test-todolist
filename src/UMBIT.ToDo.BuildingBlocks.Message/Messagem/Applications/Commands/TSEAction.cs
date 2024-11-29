using MediatR;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;

namespace UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands
{
    public interface IUMBITAction : INotification
    {

    }
    public class UMBITAction : UMBITMensagem, IUMBITAction
    {
    }
}
