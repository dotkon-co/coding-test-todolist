namespace UMBIT.ToDo.API.DTOs
{
    public class ToDoItemDTO
    {
        public Guid Id { get; set; }
        public Guid? IdToDoList { get; set; }
        public int Index { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public int Status { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
