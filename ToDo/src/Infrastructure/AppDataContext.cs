using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure
{
	public sealed class AppDataContext : DbContext
	{
		
		public DbSet<UserEntity> Users { get; set; } = null!;
		public DbSet<TodoEntity> ToDos { get; set; } = null!;

		public AppDataContext(DbContextOptions<AppDataContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			/*modelBuilder.Entity<UserEntity>().ToTable("User");
			modelBuilder.Entity<TodoEntity>().ToTable("ToDo");*/
		}
	}
}
