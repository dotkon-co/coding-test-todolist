using Domain.Requests.User.Register;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Requests.User.Login
{
	public class LoginValidator : AbstractValidator<LoginRequest>
	{
		public LoginValidator()
		{

			RuleFor(user => user.User)
				.NotEmpty().WithMessage("User is required.")
				.MaximumLength(30).WithMessage("User must not exceed 30 characters.");

			RuleFor(user => user.Password)
				.NotEmpty()
				.WithMessage("Password is required.")
				.MaximumLength(255).WithMessage("Password must not exceed 255 characters.");
		}
	}
}
