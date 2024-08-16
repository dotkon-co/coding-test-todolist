using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Todo.Entities
{
    public class User : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public byte[]? PasswordHash { get;  set; }
        public byte[]? PasswordSalt { get;  set; }
        [NotMapped]
        public string Password { get;  set; }
        public bool IsActive { get;  set; }
        public ICollection<Job>? Jobs { get; set; }
    }
}
