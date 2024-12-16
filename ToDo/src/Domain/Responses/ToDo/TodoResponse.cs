namespace Domain.Responses.ToDo
{
	public record TodoResponse(Guid Id, string Title, string Description, DateTime CreateAt, DateTime? FinishedAt, Guid UserId);
}
