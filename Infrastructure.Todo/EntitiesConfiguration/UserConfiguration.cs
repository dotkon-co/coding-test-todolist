using Core.Todo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Todo.EntitiesConfiguration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(u => u.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.HasMany(u => u.Jobs)
                .WithOne(j => j.User)
                .HasForeignKey(j => j.UserId);

            using var hmac = new System.Security.Cryptography.HMACSHA512();
            byte[] passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin123"));
            byte[] passwordSalt = hmac.Key;

            builder.HasData(new User
            {
                Id = 1,
                Name = "Admin",
                Email = "admin@teste.com.br",
                CreationDate = new DateTime(2024, 8, 16, 0, 0, 0, DateTimeKind.Utc),
                IsActive = true,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            });
        }
    }
}
