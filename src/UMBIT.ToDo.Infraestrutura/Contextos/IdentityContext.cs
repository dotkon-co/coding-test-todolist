using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UMBIT.ToDo.Dominio.Entidades.Auth.Basicos;

namespace UMBIT.ToDo.Infraestrutura.Contextos
{
    public class IdentityContext : IdentityDbContext<Usuario, Role, Guid>
    {
        public IdentityContext(DbContextOptions<IdentityContext> option) : base(option)
        {

        }
    }
}
