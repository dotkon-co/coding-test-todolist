using MediatR;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;

namespace UMBIT.ToDo.BuildingBlocks.Message.Messagem.Interfaces
{
    public interface IUMBITActionHandler<T> : INotificationHandler<T> where T : IUMBITAction
    {
    }
}
