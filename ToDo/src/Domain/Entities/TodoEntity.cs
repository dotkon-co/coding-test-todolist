namespace Domain.Entities
{
	public class TodoEntity : BaseEntity
	{
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
        public DateTime? FinishedAt { get; set; }
        public Guid UserId { get; set; }
		public virtual UserEntity User { get; set; } = null!;

		public TodoEntity(string title, string description, Guid userId)
		{
			Title = title;
			Description = description;
			UserId = userId;
		}

		public void SetUser(UserEntity user)
		{
			User = user;
		}
	}
}
