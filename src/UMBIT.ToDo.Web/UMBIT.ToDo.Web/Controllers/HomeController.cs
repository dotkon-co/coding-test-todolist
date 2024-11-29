using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.BuildingBlocksc.ASPNet.Controllers;
using UMBIT.ToDo.Web.Basicos.Enumerador;
using UMBIT.ToDo.Web.Basicos.Extensores;
using UMBIT.ToDo.Web.Models;
using UMBIT.ToDo.Web.services;
using static UMBIT.ToDo.BuildingBlocksc.ASPNet.Bootstrapper.ContextConfigurate;

namespace UMBIT.ToDo.Web.Controllers
{
    public class HomeController : ASPBaseController
    {
        private readonly IServicoToDo _servicoDeToDo;
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger, IServicoToDo servicoDeToDo, AuthSessionContext authSessionContext) : base(authSessionContext)
        {
            _logger = logger;
            _servicoDeToDo = servicoDeToDo;
        }

        public async Task<IActionResult> Index()
        {
            return await MiddlewareDeRetorno(async () =>
            {
                if (!_authSessionContext.EhAutenticado)
                {
                    return RedirectToAction("Login", "Auth");
                }

                var tasks = await this._servicoDeToDo.ObtenhaTarefasPendentes();

                if (tasks == null || !tasks.Any())
                    return RedirectToAction(nameof(IndexDemo));

                return View(tasks);
            });
        }
        public async Task<IActionResult> IndexDemo()
        {
            return await MiddlewareDeRetorno(async () =>
            {
                var idListA = Guid.NewGuid();

                var tasks = new List<TarefaDTO>
                {
                    new TarefaDTO
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        Nome = "Você pode marcar as tarefas que deseja mudar status de uma só vez",
                        Descricao = "Descrição da Task 1",
                        Status = 1,
                        DataInicio = DateTime.Now.AddDays(-1),
                        DataFim = DateTime.Now,
                    },
                    new TarefaDTO
                    {
                        Id = Guid.NewGuid(),
                        IdToDoList = idListA,
                        Index = 1,
                        Nome = "Serão mostradas as Tarefas pendentes",
                        Descricao = "Descrição da Task 1",
                        Status = 1,
                        DataInicio = DateTime.Now.AddDays(-1),
                        DataFim = DateTime.Now,
                        ToDoList = new ListaDTO { Id = idListA, Nome = "Aqui aparecerão suas Lista" }
                    },
                    new TarefaDTO
                    {
                        Id = Guid.NewGuid(),
                        IdToDoList = idListA,
                        Index = 2,
                        Nome = "Vá a tela de gerenciamento e crie novas tarefas ",
                        Descricao = "Descrição da Task 2",
                        Status = 2,
                        DataInicio = DateTime.Now.AddDays(-2),
                        DataFim = DateTime.Now.AddDays(1),
                        ToDoList = new ListaDTO { Id =idListA, Nome = "Aqui aparecerão suas Lista" }
                    },
                    new TarefaDTO
                    {
                        Id = Guid.NewGuid(),
                        IdToDoList = Guid.NewGuid(),
                        Index = 3,
                        Nome = "Caso não for o adminitrador vc só poderá ver as suas listas",
                        Descricao = "Descrição da Task 3",
                        Status = 2,
                        DataInicio = DateTime.Now,
                        DataFim = DateTime.Now.AddDays(2),
                        ToDoList = new ListaDTO { Id = Guid.NewGuid(), Nome = "Lista B" }
                    }
                };

                return View(tasks);
            });
        }

        public async Task<IActionResult> ListaTarefa(int? status = null, Guid? idList = null)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                List<TarefaDTO>? result;

                if (status != null && idList != null)
                    result = (await this._servicoDeToDo.ObtenhaTarefas(status.Value, idList.Value));
                else if (status != null)
                    result = (await this._servicoDeToDo.ObtenhaTarefas(status.Value));
                else if (idList != null)
                    result = (await this._servicoDeToDo.ObtenhaTarefas(idList.Value));
                else result = (await this._servicoDeToDo.ObtenhaTarefas());


                var lists = await this._servicoDeToDo.ObtenhaLists();

                ViewBag.Status = status;
                ViewBag.IdList = idList;
                ViewBag.Lists = lists;


                ViewBag.PieData = GerePieData(result);

                return View(result);
            });
        }

        public async Task<IActionResult> AddTask()
        {
            return await MiddlewareDeRetorno(async () =>
            {
                var lists = await this._servicoDeToDo.ObtenhaLists();
                ViewBag.Lists = lists;
                return View(new AdicionarTarefaRequest());
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddTask(AdicionarTarefaRequest taskDTO)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                if (!ModelState.IsValid)
                    return View(taskDTO);

                await this._servicoDeToDo.AdicioneItem(taskDTO);

                return RedirectToAction("ListaTarefa");

            });

        }

        public async Task<IActionResult> EditTask(Guid id)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                var lists = await this._servicoDeToDo.ObtenhaLists();
                ViewBag.Lists = lists;

                var tarefa = await this._servicoDeToDo.ObtenhaItem(id);
                return View(new AtualizarTarefaRequest()
                {
                    Status = tarefa.Status,
                    DataFim = tarefa.DataFim,
                    DataInicio = tarefa.DataInicio,
                    Id = tarefa.Id,
                    Nome = tarefa.Nome,
                    Descricao = tarefa.Descricao,
                    IdToDoList = tarefa.IdToDoList,
                    Index = tarefa.Index
                });
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditTask(AtualizarTarefaRequest taskDTO)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                if (!ModelState.IsValid)
                    return View(taskDTO);

                await this._servicoDeToDo.EditItem(taskDTO.Id, taskDTO);

                return RedirectToAction("ListaTarefa");

            });
        }

        public async Task<IActionResult> DeleteTask(Guid id)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this._servicoDeToDo.DeleteItem(id);

                return RedirectToAction("ListaTarefa");
            }, nameof(ListaTarefa));
        }
        public async Task<IActionResult> DeleteList(Guid id)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this._servicoDeToDo.DeleteList(id);

                return RedirectToAction("ListaTarefa");
            }, nameof(ListaTarefa));
        }

        public IActionResult AddList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddList(AdicionarListaRequest list)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                await this._servicoDeToDo.AdicioneList(list);
                return RedirectToAction("ListaTarefa");
            });
        }

        public async Task<IActionResult> EditList(Guid id)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                var lista = await this._servicoDeToDo.ObtenhaList(id);
                return View(new AtualizarListaRequest()
                {
                    Nome = lista.Nome,
                    Id = lista.Id,
                });
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditList(AtualizarListaRequest listTaskDTO)
        {
            return await MiddlewareDeRetorno(async () =>
            {
                if (!ModelState.IsValid)
                    return View(listTaskDTO);

                await this._servicoDeToDo.EditList(listTaskDTO.Id, listTaskDTO);

                return RedirectToAction("ListaTarefa");

            });
        }

        public async Task<IActionResult> AltereStatus([FromQuery] int status, [FromQuery] Guid id)
        {
            return await MiddlewareDeRetorno(async () =>
            {

                var item = await this._servicoDeToDo.ObtenhaItem(id);
                item.Status = status;
                await this._servicoDeToDo.EditStatus(id, new AtualizarStatusTarefaRequest()
                {
                    Id = id,
                    Status = item.Status
                });

                return RedirectToAction("ListaTarefa");

            }, nameof(ListaTarefa));
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private string GerePieData(IEnumerable<TarefaDTO>? taskDTOs)
        {
            string resultItems = "[ITEMS]";
            string items = "";

            if (taskDTOs != null && taskDTOs.Any())
            {
                var list = new List<string>();
                var groups = taskDTOs.GroupBy(t => t.Status);

                foreach (var group in groups)
                {
                    list.Add($"{{ value: {group.Count()}, name: '{EnumeradorStatus.Parse<EnumeradorStatus>(group.Key.ToString()).GetStatus()}' }} ");
                }

                items = String.Join(",", list);
            }

            return resultItems.Replace("ITEMS", items);
        }


    }
}
