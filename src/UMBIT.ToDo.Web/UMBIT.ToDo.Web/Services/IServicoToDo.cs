using Refit;
using UMBIT.Nexus.Auth.Contrato;

namespace UMBIT.ToDo.Web.services
{
    public interface IServicoToDo
    {
        [Post("/tarefa")]
        Task AdicioneItem([Body] AdicionarTarefaRequest request);

        [Put("/tarefa/{id}")]
        Task EditItem([Refit.AliasAs("id")] Guid id, [Body] AtualizarTarefaRequest request);

        [Put("/tarefa/{id}/atualizar-status")]
        Task EditStatus([Refit.AliasAs("id")] Guid id, [Body] AtualizarStatusTarefaRequest request);

        [Delete("/tarefa/{id}")]
        Task DeleteItem([Refit.AliasAs("id")] Guid id);

        [Get("/tarefa/{id}")]
        Task<TarefaDTO> ObtenhaItem([Refit.AliasAs("id")] Guid id);

        [Get("/tarefa?$filter=contains(cast(status,'Edm.String'), '{status}') AND contains(cast(idToDoList,'Edm.String'), '{idToDoList}')")]
        Task<List<TarefaDTO>?> ObtenhaTarefas([AliasAs("status")] int status, [Refit.AliasAs("idToDoList")] Guid idToDoList);

        [Get("/tarefa?$filter=contains(cast(idToDoList,'Edm.String'), '{idToDoList}')")]
        Task<List<TarefaDTO>?> ObtenhaTarefas([Refit.AliasAs("idToDoList")] Guid idToDoList);

        [Get("/tarefa?$filter=contains(cast(status,'Edm.String'), '{status}')")]
        Task<List<TarefaDTO>?> ObtenhaTarefas([AliasAs("status")] int status);

        [Get("/tarefa")]
        Task<List<TarefaDTO>?> ObtenhaTarefas();

        [Get("/tarefa?$filter=contains(cast(status,'Edm.String'), '0') OR contains(cast(status,'Edm.String'), '1') ")]
        Task<List<TarefaDTO>?> ObtenhaTarefasPendentes();

        [Post("/lista-tarefa")]
        Task AdicioneList([Body] AdicionarListaRequest request);

        [Put("/lista-tarefa/{id}")]
        Task EditList([Refit.AliasAs("id")] Guid id, [Body] AtualizarListaRequest request);

        [Delete("/lista-tarefa/{id}")]
        Task DeleteList([Refit.AliasAs("id")] Guid id);

        [Get("/lista-tarefa/{id}")]
        Task<ListaDTO> ObtenhaList([Refit.AliasAs("id")] Guid id);

        [Get("/lista-tarefa")]
        Task<List<ListaDTO>?> ObtenhaLists();
    }
}
