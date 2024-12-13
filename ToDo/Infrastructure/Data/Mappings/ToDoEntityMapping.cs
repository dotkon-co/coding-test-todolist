using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrastructure.Data.Mappings
{
	public class ToDoEntityMapping : IEntityTypeConfiguration<TodoEntity>
	{
		public void Configure(EntityTypeBuilder<TodoEntity> builder)
		{
			builder.HasKey(c => c.Id);
			builder.Property(e => e.Id).ValueGeneratedOnAdd();

			builder.HasKey(c => c.Id);

			builder.Property(c => c.Title)
				.IsRequired()
				.HasColumnType("NVARCHAR")
				.HasMaxLength(50);

			builder.Property(c => c.Description)
				.IsRequired()
				.HasColumnType("NVARCHAR")
				.HasMaxLength(500);

			builder.Property(x => x.CreatedAt)
			   .IsRequired(true);

			builder.Property(x => x.FinishedAt)
			   .IsRequired(false);

			builder.Property(x => x.UserId)
			   .IsRequired(true)
			   .HasColumnType("VARCHAR")
			   .HasMaxLength(160);

		}
	}
}
