namespace Core.Todo.Entities
{
    public class Job : BaseEntity
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Done { get; set; }
        public long UserId { get; set; }
        public User? User { get; set; }
    }
}
