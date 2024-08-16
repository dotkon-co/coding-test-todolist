using Core.Todo.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Todo.EntitiesConfiguration
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.ToTable("job");

            builder.HasKey(j => j.Id);

            builder.Property(j => j.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(j => j.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(j => j.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(j => j.Done)
                .IsRequired()
                .HasColumnType("tinyint");

        }
    }
}
