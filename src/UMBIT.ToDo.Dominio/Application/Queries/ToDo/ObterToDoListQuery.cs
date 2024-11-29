using UMBIT.ToDo.BuildingBlocks.Message.Messagem.Applications.Query;
using UMBIT.ToDo.Dominio.Entidades.ToDo;

namespace UMBIT.ToDo.Dominio.Application.Queries.ToDo
{
    public class ObterToDoListQuery : UMBITQuery<ObterToDoListQuery, IQueryable<ToDoList>>
    {
        protected override void Validadors(ValidatorQuery<ObterToDoListQuery> validator)
        {
        }
    }
}
