namespace Domain.Entities
{
	public class TodoEntity : BaseEntity
	{
		public string Title { get; set; } = null!;
		public string Description { get; set; } = null!;
        public DateTime? FinishedAt { get; set; }
        public Guid UserId { get; set; }
		public virtual UserEntity User { get; set; } = null!;

		public TodoEntity(Guid id, string title, string description, DateTime createdAt, DateTime? finishedAt) : base(id, createdAt)
		{
			Title = title;
			Description = description;
			FinishedAt = finishedAt;
		}

		public void SetUser(UserEntity user)
		{
			User = user;
		}
	}
}
