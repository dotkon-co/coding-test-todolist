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
	public class UserEntityMapping : IEntityTypeConfiguration<UserEntity>
	{
		public void Configure(EntityTypeBuilder<UserEntity> builder)
		{
			builder.ToTable("User");

			builder.HasKey(c => c.Id);
			builder.Property(e => e.Id).ValueGeneratedOnAdd();

			builder.Property(c => c.Name)
				.IsRequired()
				.HasColumnType("NVARCHAR")
				.HasMaxLength(50);

			builder.Property(c => c.User)
				.IsRequired()
				.HasColumnType("NVARCHAR")
				.HasMaxLength(30);

			builder.HasIndex(c => c.User).IsUnique();

			builder.Property(c => c.Password)
				.IsRequired()
				.HasColumnType("NVARCHAR")
				.HasMaxLength(255);

			builder.Property(x => x.CreatedAt)
			   .IsRequired(true);

		}
	}
}
