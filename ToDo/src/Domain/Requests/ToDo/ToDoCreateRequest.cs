
namespace Domain.Requests.ToDo
{
	public class ToDoCreateRequest
	{
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
	}
}
