using MediatR;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;

namespace UMBIT.ToDo.BuildingBlocks.Message.Messagem.Interfaces
{
    public interface IUMBITQueryRequestHandler<TQuery, T> : IRequestHandler<TQuery, UMBITMessageResponse<T>>
        where T : class
        where TQuery : IUMBITQuery<T>
    {
    }
}
