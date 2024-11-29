using AutoMapper;
using UMBIT.Nexus.Auth.Contrato;
using UMBIT.ToDo.Dominio.Entidades.ToDo;

namespace UMBIT.ToDo.API.Mapeadores
{
    public class ToDoMapper : Profile
    {
        public ToDoMapper()
        {
            CreateMap<TarefaDTO, ToDoItem>().ReverseMap();
            CreateMap<ListaDTO, ToDoList>().ReverseMap();
        }
    }
}
