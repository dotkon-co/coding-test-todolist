namespace Domain.Entities
{
	public class UserEntity : BaseEntity
	{
		public string Name { get; set; } = null!;
		public string User { get; set; } = null!;
		public string Password { get; set; } = null!;
		public virtual IEnumerable<TodoEntity> Todos { get; set; } = Enumerable.Empty<TodoEntity>();

		public UserEntity(string name, string user, string password)
		{
			Name = name;
			User = user;
			Password = password;
		}
	}
}
