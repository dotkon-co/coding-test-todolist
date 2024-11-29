namespace UMBIT.ToDo.API.DTOs
{
    public class ToDoListDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public List<ToDoItemDTO>? Items { get; set; }
    }
}
