namespace Domain.Entities
{
	public class UserEntity : BaseEntity
	{
		public string Name { get; set; } = null!;
		public string User { get; set; } = null!;
		public string Password { get; set; } = null!;

		public UserEntity(Guid id, string name, string user, string password, DateTime createdAt) : base(id, createdAt)
		{
			Name = name;
			User = user;
			Password = password;
		}
	}
}
