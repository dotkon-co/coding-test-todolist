using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using UMBIT.ToDo.BuildingBlocks.Basicos.Excecoes;
using UMBIT.ToDo.BuildingBlocks.Core.Notificacao.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Core.Seguranca.Models;
using UMBIT.ToDo.BuildingBlocks.Message.Bus.MessagerBus;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Commands;
using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Interfaces;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces.Database;
using UMBIT.ToDo.Dominio.Application.Commands.ToDo;
using UMBIT.ToDo.Dominio.Basicos.Enum;
using UMBIT.ToDo.Dominio.Configuradores;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;
using UMBIT.ToDo.Dominio.Entidades.ToDo;
using UMBIT.ToDo.Dominio.Workers;

namespace UMBIT.ToDo.Dominio.Application.Commands
{
    public class ToDoCommandHandler :
        UMBITCommandHandlerBase,
        IUMBITCommandRequestHandler<AdicioneToDoItemCommand>,
        IUMBITCommandRequestHandler<AdicioneToDoListCommand>,
        IUMBITCommandRequestHandler<DeleteToDoItemCommand>,
        IUMBITCommandRequestHandler<DeleteToDoListCommand>,
        IUMBITCommandRequestHandler<EditeToDoItemCommand>,
        IUMBITCommandRequestHandler<EditeToDoListItemCommand>,
        IUMBITCommandRequestHandler<AtualizarStatusToDoItemCommand>,
        IUMBITCommandRequestHandler<AviseToDoCommand>
    {
        private readonly UserManager<Usuario> _userManager;
        private readonly ContextoPrincipal _contextoPrincipal;
        private readonly IOptions<IdentitySettings> _identitySettings;
        private readonly IMessagerBus _messagerBus;

        private readonly IRepositorio<ToDoItem> RepositorioToDoItem;
        private readonly IRepositorio<ToDoList> RepositorioToDoList;

        public ToDoCommandHandler(
            IUnidadeDeTrabalhoNaoTransacional unidadeDeTrabalho,
            INotificador notificador,
            ContextoPrincipal contextoPrincipal,
            UserManager<Usuario> userManager,
            IOptions<IdentitySettings> identitySettings,
            IMessagerBus messagerBus) : base(unidadeDeTrabalho, notificador)
        {
            _userManager = userManager;
            _identitySettings = identitySettings;
            _contextoPrincipal = contextoPrincipal;
            _messagerBus = messagerBus;
            RepositorioToDoList = UnidadeDeTrabalho.ObterRepositorio<ToDoList>();
            RepositorioToDoItem = UnidadeDeTrabalho.ObterRepositorio<ToDoItem>();

        }

        public async Task<UMBITMessageResponse> Handle(AviseToDoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!(await this.RepositorioToDoItem.Query().AnyAsync(t => t.IdUsuario == request.IdUsuario && (t.Status == EnumeradorStatus.EmAndamento || t.Status == EnumeradorStatus.Criado))))
                {
                    this.AdicionarErro("O usuario não pode ser notificado pois não há tarefas pendentes");
                    return CommandResponse();
                }

                this._messagerBus.Publish<AvisoWorker.AvisoMessage>(new AvisoWorker.AvisoMessage(request.IdUsuario));
                return CommandResponse();
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao realizar aviso", ex);
            }
        }

        async Task<UMBITMessageResponse> IRequestHandler<AdicioneToDoItemCommand, UMBITMessageResponse>.Handle(AdicioneToDoItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var toDoItem = new ToDoItem()
                {
                    Nome = request.Nome,
                    Index = request.Index,
                    Descricao = request.Descricao,
                    IdToDoList = request.IdToDoList,
                    DataFim = request.DataFim,
                    DataInicio = request.DataInicio,
                    NomeUsuario = _contextoPrincipal.ObtenhaPrincipal()!.User,
                    IdUsuario = new Guid(_contextoPrincipal.ObtenhaPrincipal()!.Id),
                    Status = Enum.Parse<EnumeradorStatus>(request.Status.ToString()),
                };

                await RepositorioToDoItem.Adicionar(toDoItem);
                await UnidadeDeTrabalho.SalveAlteracoes();

                return CommandResponse();

            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao adicionar item", ex);
            }
        }

        async Task<UMBITMessageResponse> IRequestHandler<AdicioneToDoListCommand, UMBITMessageResponse>.Handle(AdicioneToDoListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var toDolist = new ToDoList();

                toDolist.Id = Guid.NewGuid();
                toDolist.Nome = request.Nome;
                toDolist.NomeUsuario = _contextoPrincipal.ObtenhaPrincipal()!.User;
                toDolist.IdUsuario = new Guid(_contextoPrincipal.ObtenhaPrincipal()!.Id);

                await RepositorioToDoList.Adicionar(toDolist);

                if (request.Items != null && request.Items.Any())
                    foreach (var toDoItemList in request.Items)
                    {
                        var toDoItem = new ToDoItem()
                        {
                            Nome = toDoItemList.Nome,
                            IdToDoList = toDolist.Id,
                            DataFim = toDoItemList.DataFim,
                            Descricao = toDoItemList.Descricao,
                            DataInicio = toDoItemList.DataInicio,
                            Index = request.Items.IndexOf(toDoItemList),
                            NomeUsuario = _contextoPrincipal.ObtenhaPrincipal()!.User,
                            IdUsuario = new Guid(_contextoPrincipal.ObtenhaPrincipal()!.Id),
                            Status = Enum.Parse<EnumeradorStatus>(toDoItemList.Status.ToString()),

                        };

                        await RepositorioToDoItem.Adicionar(toDoItem);
                    }

                await UnidadeDeTrabalho.SalveAlteracoes();
                return CommandResponse();
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao adicionar lista", ex);
            }
        }

        async Task<UMBITMessageResponse> IRequestHandler<DeleteToDoItemCommand, UMBITMessageResponse>.Handle(DeleteToDoItemCommand request, CancellationToken cancellationToken)
        {
            try
            {

                var item = await RepositorioToDoItem.Query().SingleAsync(t => t.Id == request.Id);

                if (_contextoPrincipal.ObtenhaPrincipal() != null && !_contextoPrincipal.ObtenhaPrincipal()!.EhAdministrador() && item.IdUsuario.ToString() != _contextoPrincipal.ObtenhaPrincipal()!.Id)
                {
                    AdicionarErro("O Recurso só pode ser acessado pelo usuario que o criou");
                    return CommandResponse();
                }

                RepositorioToDoItem.Remover(item);

                await UnidadeDeTrabalho.SalveAlteracoes();
                return CommandResponse();

            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao deletar tarefa", ex);
            }
        }

        async Task<UMBITMessageResponse> IRequestHandler<DeleteToDoListCommand, UMBITMessageResponse>.Handle(DeleteToDoListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var list = await RepositorioToDoList.Query().SingleAsync(t => t.Id == request.Id);

                if (_contextoPrincipal.ObtenhaPrincipal() != null && !_contextoPrincipal.ObtenhaPrincipal()!.EhAdministrador() && list.IdUsuario.ToString() != _contextoPrincipal.ObtenhaPrincipal()!.Id)
                {
                    AdicionarErro("O Recurso só pode ser acessado pelo usuario que o criou");
                    return CommandResponse();
                }

                RepositorioToDoList.Remover(list);

                await UnidadeDeTrabalho.SalveAlteracoes();
                return CommandResponse();
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao deletar lista", ex);
            }
        }

        async Task<UMBITMessageResponse> IRequestHandler<EditeToDoItemCommand, UMBITMessageResponse>.Handle(EditeToDoItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await RepositorioToDoItem.Query().SingleAsync(t => t.Id == request.Id);

                if (_contextoPrincipal.ObtenhaPrincipal() != null && !_contextoPrincipal.ObtenhaPrincipal()!.EhAdministrador() && item.IdUsuario.ToString() != _contextoPrincipal.ObtenhaPrincipal()!.Id)
                {
                    AdicionarErro("O Recurso só pode ser acessado pelo usuario que o criou");
                    return CommandResponse();
                }

                item.Nome = request.Nome;
                item.Descricao = request.Descricao;
                item.DataFim = request.DataFim;
                item.DataInicio = request.DataInicio;
                item.IdToDoList = request.IdToDoList;
                item.Status = Enum.Parse<EnumeradorStatus>(request.Status.ToString());

                RepositorioToDoItem.Atualizar(item);

                await UnidadeDeTrabalho.SalveAlteracoes();
                return CommandResponse();
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao editar tarefa", ex);
            }
        }

        async Task<UMBITMessageResponse> IRequestHandler<EditeToDoListItemCommand, UMBITMessageResponse>.Handle(EditeToDoListItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await RepositorioToDoList.Query().SingleAsync(t => t.Id == request.Id);

                if (_contextoPrincipal.ObtenhaPrincipal() != null && !_contextoPrincipal.ObtenhaPrincipal()!.EhAdministrador() && item.IdUsuario.ToString() != _contextoPrincipal.ObtenhaPrincipal()!.Id)
                {
                    AdicionarErro("O Recurso só pode ser acessado pelo usuario que o criou");
                    return CommandResponse();
                }

                item.Nome = request.Nome;

                RepositorioToDoList.Atualizar(item);

                await UnidadeDeTrabalho.SalveAlteracoes();
                return CommandResponse();
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao editar lista", ex);
            }
        }

        async Task<UMBITMessageResponse> IRequestHandler<AtualizarStatusToDoItemCommand, UMBITMessageResponse>.Handle(AtualizarStatusToDoItemCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var item = await RepositorioToDoItem.Query().SingleAsync(t => t.Id == request.Id);

                if (_contextoPrincipal.ObtenhaPrincipal() != null && !_contextoPrincipal.ObtenhaPrincipal()!.EhAdministrador() && item.IdUsuario.ToString() != _contextoPrincipal.ObtenhaPrincipal()!.Id)
                {
                    AdicionarErro("O Recurso só pode ser acessado pelo usuario que o criou");
                    return CommandResponse();
                }

                item.Status = (EnumeradorStatus)request.Status;

                RepositorioToDoItem.Atualizar(item);

                await UnidadeDeTrabalho.SalveAlteracoes();
                return CommandResponse();
            }
            catch (Exception ex)
            {
                throw new ExcecaoBasicaUMBIT("Erro ao finalizar tarefa", ex);
            }
        }
    }
}
