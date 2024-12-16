using Domain.Interfaces.Repositories;
using FluentValidation;

namespace Domain.Requests.User.Register
{
	public class RegisterValidator : AbstractValidator<RegisterRequest>
	{
		private readonly IUserRepository _userRepository;
		public RegisterValidator(IUserRepository userRepository)
		{
			_userRepository = userRepository;
			RuleFor(user => user.Name)
				.NotEmpty().WithMessage("Name is required.")
				.MaximumLength(50).WithMessage("Name must not exceed 50 characters.")
				.Matches("^[a-zA-ZÀ-ú ]*$")
				.WithMessage("Name must contain only letters.");

			RuleFor(user => user.User)
				.NotEmpty().WithMessage("User is required.")
				.MaximumLength(30).WithMessage("User must not exceed 30 characters.")
				.Matches("^[a-zA-Z][a-zA-Z0-9._]*$").WithMessage("User must start with a letter and can only contain alphanumeric characters, periods, and underscores.")
				.MustAsync(async (value, c) => await ExistAsync(value)).WithMessage((value) => $"User already exists");

			RuleFor(user => user.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MaximumLength(255).WithMessage("Password must not exceed 255 characters.");
		}

		private async Task<bool> ExistAsync(string user)
		{
			var entity = await _userRepository.GetAsync(user);
			if (entity == null)
				return true;
			return false;
		}
	}
}
