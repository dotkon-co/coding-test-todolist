using Microsoft.AspNetCore.Mvc;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Basicos.Atributos;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models;
using UMBIT.ToDo.Dominio.Application.Commands.ToDo;
using UMBIT.ToDo.Dominio.Application.Queries.ToDo;
using UMBIT.ToDo.Dominio.Entidades.ToDo;

namespace UMBIT.ToDo.API.Controllers
{
    [Autorizacao]
    public class ToDoController : ToDoControllerBase
    {
        private ContextoPrincipal _contextoPrincipal;
        public ToDoController(IServiceProvider serviceProvider, ContextoPrincipal contextoPrincipal) : base(serviceProvider)
        {
            _contextoPrincipal = contextoPrincipal;
        }

        public override async Task<ActionResult<ICollection<TarefaDTO>>> ObterTarefa()
        {
            var response = await Mediator.EnviarQuery<ObterToDoQuery, IQueryable<ToDoItem>>(new ObterToDoQuery());

            if (!_contextoPrincipal.ObtenhaPrincipal()!.EhAdministrador())
                response.Dados = response.Dados!.Where(t => t.IdUsuario.ToString() == _contextoPrincipal.ObtenhaPrincipal()!.Id);

            return UMBITCollectionResponseEntity<TarefaDTO, ToDoItem>(response);
        }
        public override async Task<IActionResult> AdicionarTarefa([FromBody] AdicionarTarefaRequest request)
        {
            var command = Mapper.Map<AdicioneToDoItemCommand>(new AdicioneToDoItemCommand()
            {
                Nome = request.Nome,
                Index = request.Index,
                Status = request.Status,
                DataFim = request.DataFim,
                Descricao = request.Descricao,
                IdToDoList = request.IdToDoList,
                DataInicio = request.DataInicio,
            });

            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        public override async Task<ActionResult<TarefaDTO>> ObterUnicaTarefa(Guid id)
        {
            var response = await Mediator.EnviarQuery<ObterToDoPorIdQuery, ToDoItem>(new ObterToDoPorIdQuery(id));
            return UMBITResponse<TarefaDTO, ToDoItem>(response);
        }

        public override async Task<IActionResult> AtualizarTarefa(Guid id, [FromBody] AtualizarTarefaRequest request)
        {
            var command = Mapper.Map<EditeToDoItemCommand>(new EditeToDoItemCommand()
            {
                Id = request.Id,
                Nome = request.Nome,
                Status = request.Status,
                DataFim = request.DataFim,
                Descricao = request.Descricao,
                DataInicio = request.DataInicio,
                IdToDoList = request.IdToDoList,
            });

            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        public override async Task<IActionResult> RemoverTarefa(Guid id)
        {
            var command = Mapper.Map<DeleteToDoItemCommand>(new DeleteToDoItemCommand()
            {
                Id = id
            });

            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        public override async Task<ActionResult<ICollection<ListaDTO>>> ObterListaTarefa()
        {
            var response = await Mediator.EnviarQuery<ObterToDoListQuery, IQueryable<ToDoList>>(new ObterToDoListQuery());

            if (!_contextoPrincipal.ObtenhaPrincipal()!.EhAdministrador())
                response.Dados = response.Dados!.Where(t => t.IdUsuario.ToString() == _contextoPrincipal.ObtenhaPrincipal()!.Id);

            return UMBITCollectionResponseEntity<ListaDTO, ToDoList>(response);
        }

        public override async Task<IActionResult> AdicionarListaTarefa([FromBody] AdicionarListaRequest request)
        {
            var command = Mapper.Map<AdicioneToDoListCommand>(new AdicioneToDoListCommand()
            {
                Nome = request.Nome,
                Items = request.Tarefas?.Select(itemDTO => (itemDTO.Nome, itemDTO.Descricao, itemDTO.DataInicio, itemDTO.DataFim, itemDTO.Status))?.ToList()
            });

            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        public override async Task<ActionResult<ListaDTO>> ObterUnicaListaTarefa(Guid id)
        {
            var response = await Mediator.EnviarQuery<ObterToDoListPorIdQuery, ToDoList>(new ObterToDoListPorIdQuery(id));
            return UMBITResponse<ListaDTO, ToDoList>(response);
        }

        public override async Task<IActionResult> AtualizarListaTarefa(Guid id, [FromBody] AtualizarListaRequest request)
        {
            var command = Mapper.Map<EditeToDoListItemCommand>(new EditeToDoListItemCommand()
            {
                Id = request.Id,
                Nome = request.Nome
            });

            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        public override async Task<IActionResult> RemoverListaTarefa(Guid id)
        {
            var command = Mapper.Map<DeleteToDoListCommand>(new DeleteToDoListCommand()
            {
                Id = id
            });

            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        [Autorizacao(true)]
        public override async Task<IActionResult> AviseTarefa(Guid id)
        {
            var command = Mapper.Map<AviseToDoCommand>(new AviseToDoCommand()
            {
                IdUsuario = id
            });

            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }

        public override async Task<IActionResult> AtualizarStatusTarefa(Guid id, [FromBody] AtualizarStatusTarefaRequest request)
        {
            var command = Mapper.Map<AtualizarStatusToDoItemCommand>(new AtualizarStatusToDoItemCommand()
            {
                Id = request.Id,
                Status = request.Status
            });

            var response = await Mediator.EnviarComando(command);

            return UMBITResponse(response);
        }
    }
}
