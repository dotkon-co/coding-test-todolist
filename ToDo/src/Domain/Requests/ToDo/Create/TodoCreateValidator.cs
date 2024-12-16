using FluentValidation;

namespace Domain.Requests.ToDo.Create
{
	
	public class TodoCreateValidator : AbstractValidator<ToDoCreateRequest>
	{
		public TodoCreateValidator()
		{
			RuleFor(todo => todo.Title)
				.NotEmpty()
				.WithMessage("Title is required.")
				.MaximumLength(50).WithMessage("Title must not exceed 50 characters.");

			RuleFor(todo => todo.Description)
				.NotEmpty()
				.WithMessage("Description is required.")
				.MaximumLength(500).WithMessage("UsDescriptioner must not exceed 500 characters.");
		}
	}
}
