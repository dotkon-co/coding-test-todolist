using Core.Todo.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Todo.Context
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) 
        { 
        }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<User> Users { get; set; }
        
    }

}
