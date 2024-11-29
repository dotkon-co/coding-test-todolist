using MediatR;
using Microsoft.EntityFrameworkCore;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;
using UMBIT.ToDo.Dominio.Application.Queries.ToDo;
using UMBIT.ToDo.Dominio.Entidades.ToDo;

namespace UMBIT.ToDo.Dominio.Application.Queries
{
    public class ToDoQueryHandler :
        UMBITQueryHandlerBase,
        IUMBITQueryRequestHandler<ObterToDoPorIdQuery, ToDoItem>,
        IUMBITQueryRequestHandler<ObterToDoQuery, IQueryable<ToDoItem>>,
        IUMBITQueryRequestHandler<ObterToDoListPorIdQuery, ToDoList>,
        IUMBITQueryRequestHandler<ObterToDoListQuery, IQueryable<ToDoList>>
    {

        private readonly IRepositorioDeLeitura<ToDoItem> RepositorioToDoItem;
        private readonly IRepositorioDeLeitura<ToDoList> RepositorioToDoList;
        public ToDoQueryHandler(IUnidadeDeTrabalhoDeLeitura unidadeDeTrabalho, INotificador notificador) : base(unidadeDeTrabalho, notificador)
        {
            RepositorioToDoList = UnidadeDeTrabalho.ObterRepositorio<ToDoList>();
            RepositorioToDoItem = UnidadeDeTrabalho.ObterRepositorio<ToDoItem>();
        }

        async Task<UMBITMessageResponse<ToDoItem>> IRequestHandler<ObterToDoPorIdQuery, UMBITMessageResponse<ToDoItem>>.Handle(ObterToDoPorIdQuery request, CancellationToken cancellationToken)
        {
            var result = await this.RepositorioToDoItem.Query().Include(t => t.ToDoList).SingleOrDefaultAsync(t => t.Id == request.Id);
            return QueryResponse(result);
        }

        Task<UMBITMessageResponse<IQueryable<ToDoItem>>> IRequestHandler<ObterToDoQuery, UMBITMessageResponse<IQueryable<ToDoItem>>>.Handle(ObterToDoQuery request, CancellationToken cancellationToken)
        {
            var result = this.RepositorioToDoItem.Query().Include(t => t.ToDoList).AsQueryable();
            return TaskQueryResponse(result);
        }

        async Task<UMBITMessageResponse<ToDoList>> IRequestHandler<ObterToDoListPorIdQuery, UMBITMessageResponse<ToDoList>>.Handle(ObterToDoListPorIdQuery request, CancellationToken cancellationToken)
        {
            var result = await this.RepositorioToDoList.Query().SingleOrDefaultAsync(t => t.Id == request.Id);
            return QueryResponse(result);
        }

        Task<UMBITMessageResponse<IQueryable<ToDoList>>> IRequestHandler<ObterToDoListQuery, UMBITMessageResponse<IQueryable<ToDoList>>>.Handle(ObterToDoListQuery request, CancellationToken cancellationToken)
        {
            var result = this.RepositorioToDoList.Query();
            return TaskQueryResponse(result);
        }
    }
}
