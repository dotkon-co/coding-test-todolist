namespace Domain.Requests.ToDo.Create
{
    public class ToDoCreateRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
