using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UMBIT.ToDo.BuildingBlocks.Repositorio.Interfaces;

namespace UMBIT.ToDo.BuildingBlocks.Repositorio.EF
{
    public abstract class CoreEntityConfigurate<T> : IEntityTypeConfiguration<T> where T : class, IBaseEntity
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey((be) => be.Id);
            ConfigureEntidade(builder);
            builder.Property((be) => be.DataCriacao).IsRequired();
            builder.Property((be) => be.DataAtualizacao).IsRequired();
        }

        public abstract void ConfigureEntidade(EntityTypeBuilder<T> builder);
    }
    public abstract class CoreEntityConfigurateCustom<T> : IEntityTypeConfiguration<T> where T : class
    {
        public void Configure(EntityTypeBuilder<T> builder)
        {
            ConfigureEntidade(builder);
        }

        public abstract void ConfigureEntidade(EntityTypeBuilder<T> builder);
    }
}
