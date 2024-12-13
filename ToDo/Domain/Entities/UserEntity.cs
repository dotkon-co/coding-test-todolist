namespace Domain.Entities
{
	public class UserEntity : BaseEntity
	{
		public string Name { get; set; } = null!;
		public string User { get; set; } = null!;
		public string Password { get; set; } = null!;

		public UserEntity(string name, string user, string password)
		{
			Name = name;
			User = user;
			Password = password;
		}
	}
}
