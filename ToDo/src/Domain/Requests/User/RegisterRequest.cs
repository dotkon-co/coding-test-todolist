
namespace Domain.Requests.User
{
	public class RegisterRequest
	{
		public string Name { get; set; } = null!;
		public string User { get; set; } = null!;
		public string Password { get; set; } = null!;
	}
}
