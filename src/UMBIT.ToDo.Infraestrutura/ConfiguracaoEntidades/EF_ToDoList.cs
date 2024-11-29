using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UMBIT.ToDo.BuildingBlocks.Repositorio.EF;
using UMBIT.ToDo.Dominio.Entidades.ToDo;


namespace UMBIT.ToDo.Infraestrutura.ConfiguracaoEntidades
{
    public class EF_ToDoList : CoreEntityConfigurate<ToDoList>
    {
        public override void ConfigureEntidade(EntityTypeBuilder<ToDoList> builder)
        {
            builder.HasMany(t => t.ToDoItems)
                .WithOne(t => t.ToDoList)
                .HasForeignKey(t => t.IdToDoList)
                .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
                
        }
    }
}
