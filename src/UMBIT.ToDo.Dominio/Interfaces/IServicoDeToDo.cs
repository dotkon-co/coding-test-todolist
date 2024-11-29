using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMBIT.ToDo.Dominio.Entidades.ToDo;

namespace UMBIT.ToDo.Dominio.Interfaces
{
    public interface IServicoDeToDo
    {
        Task DeleteToDoList(Guid id);
        Task DeleteToDoItem(Guid id);

        Task AdicioneToDoItem(ToDoItem toDoItem);
        Task AdicioneToDoList(string nome, List<ToDoItem> toDoItems);

        Task EditeToDoItem(Guid id, Guid? idToDoList, DateTime datafim, DateTime datainicio, string nome, string descricao, int status);
        Task EditeToDoListItem(Guid id, string nome);
        Task FinalizeToDoItem(Guid id);

        Task<ToDoItem> ObtenhaToDoItem(Guid id);
        Task<IEnumerable<ToDoItem>> ObtenhaToDoItems();

        Task<ToDoList> ObtenhaToDoList(Guid id);
        Task<IEnumerable<ToDoList>> ObtenhaToDoLists();
    }
}
